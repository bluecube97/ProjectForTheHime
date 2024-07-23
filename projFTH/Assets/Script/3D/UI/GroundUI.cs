using Script._3D.Lib;
using Script._3D.Player;
using Script.ApiLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script._3D.UI
{
    public class GroundUI : MonoBehaviour
    {
        private PlayerManager _pm;
        private PositionComponentVo _pcvo;

        public GameObject placeBtn;
        public GameObject placeBtnLayout;
        public int linePlaceBtnCnt;
        public GameObject player;
        public int startPlayerPosX;
        public int startPlayerPosZ;
        public GameObject canvas;

        public GameObject cam; // 카메라
        private SuperBlur.SuperBlur _blur; // 블러

        public GameObject uiCanvas; // UI 캔버스
        public Button diceBtn; // 주사위 버튼
        public Text diceValueTxt; // 주사위 값 텍스트

        private int MoveDistance { get; set; } // 이동 거리

        private Node[,] _nodes;

        protected virtual void Awake()
        {
            Time.timeScale = 1.0f;
            _nodes = new Node[linePlaceBtnCnt, linePlaceBtnCnt];

            // 노드 초기화
            for (int i = 0; i < linePlaceBtnCnt; i++)
            {
                for (int j = 0; j < linePlaceBtnCnt; j++)
                {
                    int posX = i - linePlaceBtnCnt / 2;
                    int posZ = j - linePlaceBtnCnt / 2;
                    _nodes[i, j] = new Node(new Vector2Int(posX, posZ), null);
                }
            }

            // 노드 이웃 설정
            for (int i = 0; i < linePlaceBtnCnt; i++)
            {
                for (int j = 0; j < linePlaceBtnCnt; j++)
                {
                    if (i > 0) _nodes[i, j].Neighbors.Add(_nodes[i - 1, j]);
                    if (i < linePlaceBtnCnt - 1) _nodes[i, j].Neighbors.Add(_nodes[i + 1, j]);
                    if (j > 0) _nodes[i, j].Neighbors.Add(_nodes[i, j - 1]);
                    if (j < linePlaceBtnCnt - 1) _nodes[i, j].Neighbors.Add(_nodes[i, j + 1]);
                }
            }

            // UI 설정
            int maxPlaceBtn = linePlaceBtnCnt * linePlaceBtnCnt;
            RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
            canvasRectTransform.sizeDelta = new Vector2(70 + 50 * linePlaceBtnCnt + 5 * (linePlaceBtnCnt - 1),
                70 + 50 * linePlaceBtnCnt + 5 * (linePlaceBtnCnt - 1));

            for (int i = 0; i < maxPlaceBtn; i++)
            {
                GameObject placeBtnInstance = Instantiate(placeBtn, placeBtnLayout.transform);
                PositionComponentVo positionComponent = placeBtnInstance.GetComponent<PositionComponentVo>();
                positionComponent.posX = (i % linePlaceBtnCnt) - (linePlaceBtnCnt / 2);
                positionComponent.posZ = (i / linePlaceBtnCnt) - (linePlaceBtnCnt / 2);
                Text placeBtnTxtComponent = placeBtnInstance.GetComponentInChildren<Text>();
                placeBtnTxtComponent.text = positionComponent.posX + ", " + positionComponent.posZ;

                // 노드에 PositionComponentVo 연결
                _nodes[positionComponent.posX + linePlaceBtnCnt / 2, positionComponent.posZ + linePlaceBtnCnt / 2].PositionComponent = positionComponent;
            }

            placeBtn.SetActive(false);

            // 플레이어 위치 설정
            PositionComponentVo playerPositionComponent = player.GetComponent<PositionComponentVo>();
            playerPositionComponent.posX = startPlayerPosX;
            playerPositionComponent.posZ = startPlayerPosZ;

            Vector3 startPosition = new Vector3(playerPositionComponent.posX * 5.5f, 1.1f, playerPositionComponent.posZ * -5.5f);
            player.transform.position = startPosition;
            player.GetComponent<PlayerManager>().targetPosition = startPosition;

            _blur = cam.GetComponent<SuperBlur.SuperBlur>();
        }

        private void Start()
        {
            _pm = player.GetComponent<PlayerManager>();
            SetPlaceBtnDistance();
            DicePhase();
        }

        public void SetPlaceBtnDistance()
        {
            int playerX = player.GetComponent<PositionComponentVo>().posX;
            int playerZ = player.GetComponent<PositionComponentVo>().posZ;

            // nodes 배열에서 올바른 노드를 참조하도록 인덱스 변환
            Node start = _nodes[playerX + linePlaceBtnCnt / 2, playerZ + linePlaceBtnCnt / 2];

            // 각 버튼에 대해 Dijkstra 알고리즘을 사용하여 이동 가능한 거리를 계산
            foreach (Transform child in placeBtnLayout.transform)
            {
                PositionComponentVo positionComponent = child.GetComponent<PositionComponentVo>();
                if (positionComponent == null) continue;

                Node target = _nodes[positionComponent.posX + linePlaceBtnCnt / 2, positionComponent.posZ + linePlaceBtnCnt / 2];
                int distance = GetDistance(start, target);

                Text childTxt = child.GetComponentInChildren<Text>();
                if (distance <= _pm.MoveCnt && !positionComponent.isBlock)
                {
                    child.GetComponent<Button>().interactable = true;
                    childTxt.text = Convert.ToString(distance);
                }
                else
                {
                    child.GetComponent<Button>().interactable = false;
                    childTxt.text = "";
                }
            }
        }

        public int GetDistance(Node start, Node target)
        {
            if (start == target)
                return 0;

            // Dijkstra 알고리즘을 사용하여 start 노드에서 target 노드까지의 최단 거리를 계산
            foreach (Node node in _nodes)
            {
                if (node == null) continue;
                node.Distance = int.MaxValue;
                node.Visited = false;
                node.Previous = null;
            }

            start.Distance = 0;
            PriorityQueue<Node> queue = new PriorityQueue<Node>();
            queue.Enqueue(start, start.Distance);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();
                current.Visited = true;

                if (current == target)
                {
                    return current.Distance;
                }

                foreach (Node neighbor in current.Neighbors)
                {
                    if (neighbor == null || neighbor.Visited || (neighbor.PositionComponent != null && neighbor.PositionComponent.isBlock)) continue;
                    int tentativeDistance = current.Distance + 1;
                    if (tentativeDistance < neighbor.Distance)
                    {
                        neighbor.Distance = tentativeDistance;
                        neighbor.Previous = current;
                        queue.Enqueue(neighbor, neighbor.Distance);
                    }
                }
            }

            return int.MaxValue; // 도달할 수 없는 경우
        }

        public void OnClickPlaceBtn(Button button)
        {
            int btnX = button.GetComponent<PositionComponentVo>().posX;
            int btnZ = button.GetComponent<PositionComponentVo>().posZ;

            int playerX = player.GetComponent<PositionComponentVo>().posX;
            int playerZ = player.GetComponent<PositionComponentVo>().posZ;

            // nodes 배열에서 올바른 노드를 참조하도록 인덱스 변환
            Node start = _nodes[playerX + linePlaceBtnCnt / 2, playerZ + linePlaceBtnCnt / 2];
            Node target = _nodes[btnX + linePlaceBtnCnt / 2, btnZ + linePlaceBtnCnt / 2];

            Debug.Log("start: " + start.Position);
            Debug.Log("target: " + target.Position);

            // 이동 거리 계산
            List<Node> path = Dijkstra(start, target);
            if (path != null)
            {
                MoveDistance = path.Count - 1;

                if (_pm.MoveCnt < MoveDistance)
                {
                    Debug.LogWarning("Not enough move points to reach the target");
                    return; // 이동 포인트가 부족할 경우 이동하지 않음
                }

                _pm.MoveCnt -= MoveDistance;
                Debug.Log("MoveCnt: " + _pm.MoveCnt);

                // 목표 위치를 positionComponentVo의 좌표로 설정
                player.GetComponent<PlayerManager>().targetPosition = new Vector3(btnX * 5.5f, 1.1f, btnZ * -5.5f);

                _pm.SetPath(path);
            }
            else
            {
                Debug.LogError("Failed to find path from " + start.Position + " to " + target.Position);
            }
        }

        private List<Node> Dijkstra(Node start, Node target)
        {
            // 모든 노드 초기화
            foreach (Node node in _nodes)
            {
                if (node == null) continue;
                node.Distance = int.MaxValue;
                node.Visited = false;
                node.Previous = null;
            }

            start.Distance = 0;
            PriorityQueue<Node> queue = new PriorityQueue<Node>();
            queue.Enqueue(start, start.Distance);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();
                current.Visited = true;

                Debug.Log("current: " + current.Position);

                if (current == target)
                {
                    List<Node> path = new List<Node>();
                    while (current != null)
                    {
                        path.Add(current);
                        current = current.Previous;
                    }
                    path.Reverse();
                    Debug.Log("Found path from " + start.Position + " to " + target.Position);
                    return path;
                }

                foreach (Node neighbor in current.Neighbors)
                {
                    if (neighbor == null || neighbor.Visited || (neighbor.PositionComponent != null && neighbor.PositionComponent.isBlock)) continue;
                    int tentativeDistance = current.Distance + 1; // Assuming each edge has weight 1
                    if (tentativeDistance >= neighbor.Distance) continue;

                    neighbor.Distance = tentativeDistance;
                    neighbor.Previous = current;
                    queue.Enqueue(neighbor, neighbor.Distance);
                }
            }

            Debug.LogWarning("Failed to find path from " + start.Position + " to " + target.Position);
            return null;
        }

        private void DisableBlur()
        {
            _blur.interpolation = 0;
            _blur.downsample = 0;
        }

        private void EnableBlur()
        {
            _blur.interpolation = 0.8f;
            _blur.downsample = 1;
        }

        public void DicePhase()
        {
            EnableBlur();
            uiCanvas.SetActive(true);
            diceValueTxt.text = "";
        }

        private void MovePhase()
        {
            DisableBlur();
            uiCanvas.SetActive(false);
        }

        public void OnClickDiceBtn(Button button)
        {
            if (button.GetComponentInChildren<Text>().text == "확인")
            {
                button.GetComponentInChildren<Text>().text = "굴리기!";
                MovePhase();
                SetPlaceBtnDistance();
            }
            else if (button.GetComponentInChildren<Text>().text == "굴리기!")
            {
                StartCoroutine(SetDiceValue(0.2f));
            }
        }

        private IEnumerator SetDiceValue(float time)
        {
            diceBtn.interactable = false;
            diceValueTxt.text = "주사위를 굴리는 중.";
            yield return new WaitForSeconds(time);
            diceValueTxt.text = "주사위를 굴리는 중..";
            yield return new WaitForSeconds(time);
            diceValueTxt.text = "주사위를 굴리는 중...";
            yield return new WaitForSeconds(time);

            int diceValue = UnityEngine.Random.Range(1, 16);
            diceValueTxt.text = diceValue.ToString();
            _pm.MoveCnt = diceValue;
            diceBtn.interactable = true;
            diceBtn.GetComponentInChildren<Text>().text = "확인";
        }
    }
}

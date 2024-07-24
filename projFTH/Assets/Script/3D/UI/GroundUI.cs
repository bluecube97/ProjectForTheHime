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
        private PlayerManager _pm; // 플레이어 매니저
        private PositionComponentVo _pcvo; // 위치 컴포넌트 VO

        public GameObject placeBtn; // 장소 버튼
        public GameObject placeBtnLayout; // 장소 버튼 레이아웃
        public int linePlaceBtnCnt; // 한 줄에 배치할 버튼 수
        public GameObject player; // 플레이어 오브젝트
        public int startPlayerPosX; // 시작 플레이어 위치 X
        public int startPlayerPosZ; // 시작 플레이어 위치 Z
        public GameObject canvas; // 캔버스 오브젝트

        public GameObject cam; // 카메라 오브젝트
        private SuperBlur.SuperBlur _blur; // 블러 효과

        public GameObject uiCanvas; // UI 캔버스
        public Button diceBtn; // 주사위 버튼
        public Text diceValueTxt; // 주사위 값 텍스트

        private int MoveDistance { get; set; } // 이동 거리

        private Node[,] _nodes; // 노드 배열

        protected virtual void Awake()
        {
            // 시간 스케일을 1로 설정 (모든 환경에서 동일한 이동속도를 구현하기 위함)
            Time.timeScale = 1.0f;
            // 노드 배열 초기화
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

            // 이웃 노드 설정
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

            // 바닥 타일 UI 설정 및 생성
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
            // 부모 버튼 비활성화
            placeBtn.SetActive(false);

            // 플레이어 시작 위치 설정
            PositionComponentVo playerPositionComponent = player.GetComponent<PositionComponentVo>();
            playerPositionComponent.posX = startPlayerPosX;
            playerPositionComponent.posZ = startPlayerPosZ;

            Vector3 startPosition = new(playerPositionComponent.posX * 5.5f, 1.1f, playerPositionComponent.posZ * -5.5f);
            player.transform.position = startPosition;
            player.GetComponent<PlayerManager>().targetPosition = startPosition;
            // 블러 호출
            _blur = cam.GetComponent<SuperBlur.SuperBlur>();
        }

        private void Start()
        {
            // PlayerManager 컴포넌트 참조
            _pm = player.GetComponent<PlayerManager>();
            SetPlaceBtnDistance();
            DicePhase();
        }

        // 현재 위치와 타일 사이의 거리 계산
        public void SetPlaceBtnDistance()
        {
            // 플레이어의 현재 위치
            int playerX = player.GetComponent<PositionComponentVo>().posX;
            int playerZ = player.GetComponent<PositionComponentVo>().posZ;

            // nodes 배열에서 올바른 노드를 참조하도록 인덱스 변환
            Node start = _nodes[playerX + linePlaceBtnCnt / 2, playerZ + linePlaceBtnCnt / 2];

            // 이동 가능한 각 버튼에 대해 거리를 계산
            foreach (Transform child in placeBtnLayout.transform)
            {
                PositionComponentVo positionComponent = child.GetComponent<PositionComponentVo>();
                if (positionComponent == null) continue;
                // 거리를 계산 할 노드 설정
                Node target = _nodes[positionComponent.posX + linePlaceBtnCnt / 2, positionComponent.posZ + linePlaceBtnCnt / 2];
                // 거리 계산
                int distance = GetDistance(start, target);
                // 버튼에 거리 표시
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
        // Dijkstra 알고리즘을 사용하여 start 노드에서 target 노드까지의 최단 거리를 계산
        private int GetDistance(Node start, Node target)
        {
            // 시작 노드와 목표 노드가 같은 경우 거리는 0
            if (start == target)
                return 0;
            // 모든 노드 초기화
            foreach (Node node in _nodes)
            {
                if (node == null) continue;
                node.Distance = int.MaxValue;
                node.Visited = false;
                node.Previous = null;
            }
            // 시작 노드의 거리는 0
            start.Distance = 0;
            // 우선순위 큐 생성
            PriorityQueue<Node> queue = new();
            // 시작 노드를 큐에 추가
            queue.Enqueue(start, start.Distance);
            // 큐가 빌 때 까지 반복
            while (queue.Count > 0)
            {
                // 큐에서 노드를 가져옴
                Node current = queue.Dequeue();
                // 노드 방문 표시
                current.Visited = true;
                // 목표 노드에 도달하면 거리 반환
                if (current == target) return current.Distance;
                // 이웃 노드 순회
                foreach (Node neighbor in current.Neighbors)
                {
                    if (neighbor == null || neighbor.Visited || (neighbor.PositionComponent != null && neighbor.PositionComponent.isBlock)) continue;
                    int tentativeDistance = current.Distance + 1;
                    if (tentativeDistance >= neighbor.Distance) continue;
                    neighbor.Distance = tentativeDistance;
                    neighbor.Previous = current;
                    queue.Enqueue(neighbor, neighbor.Distance);
                }
            }
            // 도달할 수 없는 경우
            return int.MaxValue;
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
            PriorityQueue<Node> queue = new();
            queue.Enqueue(start, start.Distance);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();
                current.Visited = true;

                Debug.Log("current: " + current.Position);

                if (current == target)
                {
                    List<Node> path = new();
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
                    int tentativeDistance = current.Distance + 1;
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

            int diceValue = UnityEngine.Random.Range(1, 7);
            diceValueTxt.text = diceValue.ToString();
            _pm.MoveCnt = diceValue;
            diceBtn.interactable = true;
            diceBtn.GetComponentInChildren<Text>().text = "확인";
        }
    }
}

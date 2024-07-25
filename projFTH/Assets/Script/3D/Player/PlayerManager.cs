using Script._3D.Lib;
using Script._3D.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script._3D.Player
{
    public class PlayerManager : MonoBehaviour
    {
        private GroundUI _groundUI;
        private BattleUI _battleUI;

        public float speed; // 이동속도
        private Node _targetNode; // 목표 노드
        private List<Node> _path; // 이동 경로
        private Coroutine _moveCoroutine; // 이동 코루틴

        public Vector3 targetPosition; // 이동 할 목표 좌표
        public GameObject player; // 플레이어
        private PositionComponentVo _playerPositionComponent; // 플레이어의 위치 컴포넌트

        public bool isMoving; // 이동중인지 여부

        public int MoveCnt { get; set; } // 이동 가능한 포인트

        private void Start()
        {
            _groundUI = FindObjectOfType<GroundUI>();
            _battleUI = FindObjectOfType<BattleUI>();
            MoveCnt = 0;
            // 플레이어의 초기 위치를 목적지로 설정
            targetPosition = transform.position;
            _playerPositionComponent = player.GetComponent<PositionComponentVo>();
        }
        // 이동할 경로를 설정하는 메서드
        public void SetPath(List<Node> newPath, int encounterCnt)
        {
            // 이동 중이면 새로운 경로를 설정하지 않음
            if (isMoving)
            {
                Debug.Log("Already moving, cannot set new path");
                return;
            }
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine); // 기존 이동 코루틴 중단
                _moveCoroutine = null; // 코루틴 참조 초기화
            }
            // 새로운 경로 설정
            _path = newPath;
            _moveCoroutine = StartCoroutine(MoveAlongPath(encounterCnt));
        }

        private IEnumerator MoveAlongPath(int encounterCnt)
        {
            if (_path == null)
            {
                Debug.Log("Failed to find path");
                yield break;
            }

            isMoving = true;
            // 움직이는 동안 화면 클릭 방지
            _groundUI.dontTouchCanvas.SetActive(true);

            if (encounterCnt > 0)
            {
                for (int i = 1; i < encounterCnt + 1; i++) // 첫 번째 노드는 건너뜀
                {
                    Node node = _path[i];
                    Vector3 targetPosition = new(node.Position.x * 5.5f, transform.position.y, node.Position.y * -5.5f);
                    while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                    {
                        transform.position =
                            Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                        yield return null;
                    }
                }
                // 위치 업데이트
                _playerPositionComponent.posX = _path[encounterCnt].Position.x;
                _playerPositionComponent.posZ = _path[encounterCnt].Position.y;
            }
            else
            {
                for (int i = 1; i < _path.Count; i++) // 첫 번째 노드는 건너뜀
                {
                    Node node = _path[i];
                    Vector3 targetPosition = new(node.Position.x * 5.5f, transform.position.y, node.Position.y * -5.5f);
                    while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                    {
                        transform.position =
                            Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                        yield return null;
                    }
                }
                // 위치 업데이트
                _playerPositionComponent.posX = _path[^1].Position.x;
                _playerPositionComponent.posZ = _path[^1].Position.y;
            }

            isMoving = false;
            _path = null; // 경로 초기화

            _groundUI.SetPlaceBtnDistance();
            // 움직이는 동안 화면 클릭 방지 해제
            _groundUI.dontTouchCanvas.SetActive(false);

            if (encounterCnt > 0)
            {
                _battleUI.StartBattle();
            }

            if (MoveCnt == 0)
            {
                _groundUI.DicePhase();
            }
        }
    }
}

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

        public float speed; // 이동속도
        private Node _targetNode;
        private List<Node> _path;
        private Coroutine _moveCoroutine; // 이동 코루틴

        public Vector3 targetPosition; // 이동 할 목표 좌표
        public GameObject player;
        private PositionComponentVo _playerPositionComponent;

        public bool isMoving; // 이동중인지 여부

        public int MoveCnt { get; set; }

        private void Start()
        {
            _groundUI = FindObjectOfType<GroundUI>();
            MoveCnt = 0;
            // 플레이어의 초기 위치를 목적지로 설정
            targetPosition = transform.position;
            _playerPositionComponent = player.GetComponent<PositionComponentVo>();
        }

        public void SetPath(List<Node> newPath)
        {
            if (isMoving)
            {
                Debug.Log("Already moving, cannot set new path");
                return; // 이동 중이면 새로운 경로를 설정하지 않습니다.
            }

            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine); // 기존 이동 코루틴 중단
                _moveCoroutine = null; // 코루틴 참조 초기화
            }

            _path = newPath;
            _moveCoroutine = StartCoroutine(MoveAlongPath());
        }

        private IEnumerator MoveAlongPath()
        {
            if (_path == null)
            {
                Debug.Log("Failed to find path");
                yield break;
            }

            isMoving = true;

            for (int i = 1; i < _path.Count; i++) // 첫 번째 노드는 건너뜁니다.
            {
                Node node = _path[i];
                Vector3 targetPosition = new Vector3(node.Position.x * 5.5f, transform.position.y, node.Position.y * -5.5f);
                while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                    yield return null;
                }
            }

            // 위치 업데이트
            _playerPositionComponent.posX = _path[_path.Count - 1].Position.x;
            _playerPositionComponent.posZ = _path[_path.Count - 1].Position.y;

            isMoving = false;
            _path = null; // 경로 초기화

            _groundUI.SetPlaceBtnDistance();

            if (MoveCnt == 0)
            {
                _groundUI.DicePhase();
            }
        }
    }
}

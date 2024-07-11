using Script._3D.UI;
using System;
using UnityEngine;

namespace Script._3D.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public float speed; // 이동속도
        public Vector3 targetPosition; // 이동 할 목표 좌표
        public GameObject player;
        private PositionComponentVo _playerPositionComponent;

        public int moveCnt = 0; // 이동 가능 횟수

        private void Start()
        {
            // 플레이어의 초기 위치를 목적지로 설정
            targetPosition = transform.position;
            _playerPositionComponent = player.GetComponent<PositionComponentVo>();
        }

        private void Update()
        {
            // 플레이어 목표 좌표 설정
            float targetX = targetPosition.x;
            float targetZ = targetPosition.z;
            // 플레이어 현재 위치 설정
            float playerX = player.transform.position.x;
            float playerZ = player.transform.position.z;
            // 목표와 플레이어 사이의 거리 계산
            float distanceX = Mathf.Abs(targetX - playerX);
            float distanceZ = Mathf.Abs(targetZ - playerZ);
            // 이동 방향 선언
            Vector3 direction;
            // X축 거리가 짧을 경우
            if (distanceX <= distanceZ)
            {
                // 목표에 도달하지 않았을 경우
                if (!IsAtTargetPositionX() && !IsAtTargetPositionZ())
                {
                    direction = targetX > playerX ? Vector3.right.normalized : Vector3.left.normalized;
                    transform.Translate(direction * (Time.deltaTime * speed), Space.World);
                }
                // X축만 목표에 도달했을 경우
                else if (IsAtTargetPositionX() && !IsAtTargetPositionZ())
                {
                    direction = targetZ > playerZ ? Vector3.forward.normalized : Vector3.back.normalized;
                    transform.Translate(direction * (Time.deltaTime * speed), Space.World);
                }
                // 목적지에 근접하면 이동을 멈추고, PositionComponentVo에 좌표 입력
                else if (IsAtTargetPositionX() && IsAtTargetPositionZ())
                {
                    _playerPositionComponent.posX = Convert.ToInt32(targetPosition.x / 5.5f);
                    _playerPositionComponent.posZ = Convert.ToInt32(-targetPosition.z / 5.5f);
                }
            }
            // Z축 거리가 짧을 경우
            else
            {
                // 목표에 도달하지 않았을 경우
                if (!IsAtTargetPositionX() && !IsAtTargetPositionZ())
                {
                    direction = targetZ > playerZ ? Vector3.forward.normalized : Vector3.back.normalized;
                    transform.Translate(direction * (Time.deltaTime * speed), Space.World);
                }
                // Z축만 목표에 도달했을 경우
                else if (!IsAtTargetPositionX() && IsAtTargetPositionZ())
                {
                    direction = targetX > playerX ? Vector3.right.normalized : Vector3.left.normalized;
                    transform.Translate(direction * (Time.deltaTime * speed), Space.World);
                }
                // 목적지에 근접하면 이동을 멈추고, PositionComponentVo에 좌표 입력
                else if (IsAtTargetPositionX() && IsAtTargetPositionZ())
                {
                    _playerPositionComponent.posX = Convert.ToInt32(targetPosition.x / 5.5f);
                    _playerPositionComponent.posZ = Convert.ToInt32(-targetPosition.z / 5.5f);
                }
            }
        }

        // 목표 좌표의 X, Z축과의 거리가 각각 0.2f 이하인지 확인 (근접 여부)
        private bool IsAtTargetPositionX()
        {
            return Mathf.Abs(transform.position.x - targetPosition.x) < 0.2f;
        }

        private bool IsAtTargetPositionZ()
        {
            return Mathf.Abs(transform.position.z - targetPosition.z) < 0.2f;
        }
    }
}
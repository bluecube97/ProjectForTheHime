using UnityEngine;
using UnityEngine.Serialization;

namespace Script._3D.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public float speed; // 이동속도
        public Vector3 targetPosition; // 이동 할 목표 좌표

        public GameObject uiCanvas;

        public GameObject cam; // 카메라
        private SuperBlur.SuperBlur _blur; // 블러

        private Rigidbody _rb;

        private void Start()
        {
            _blur = cam.GetComponent<SuperBlur.SuperBlur>();
            _rb = GetComponent<Rigidbody>();
            targetPosition = transform.position;

            DicePhase();
        }

        private void Update()
        {
            // 목적지에 근접하면 이동을 멈춤
            if (IsAtTargetPosition()) return;
            // 플레이어 이동
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.Translate(direction * (Time.deltaTime * speed), Space.World);
        }

        // 목적지와의 거리 차이가 0.2f 이하인지 확인
        private bool IsAtTargetPosition()
        {
            return Vector3.Distance(transform.position, targetPosition) < 0.2f;
        }

        // blur 비활성화
        private void DisableBlur()
        {
            _blur.interpolation = 0;
            _blur.downsample = 0;
        }
        // blur 활성화
        private void EnableBlur()
        {
            _blur.interpolation = 0.8f;
            _blur.downsample = 1;
        }

        private void DicePhase()
        {
            EnableBlur();
        }
    }
}
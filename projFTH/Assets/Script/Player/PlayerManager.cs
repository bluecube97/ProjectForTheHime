using System;
using UnityEngine;

namespace Script.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public float speed;
        public float jumpForce;

        private int _jumpCount;

        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // 모든 방향의 속도를 일정하게 만들기 위해 정규화
            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            // 플레이어 이동
            //  transform.Translate(direction * (Time.deltaTime * speed));
            // 물리 작용을 사용해 이동
            _rb.MovePosition(_rb.position + (direction * (Time.deltaTime * speed)));

            // 플레이어 점프
            if (Input.GetKeyDown(KeyCode.Space) && _jumpCount < 2)
            {
                // 점프
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                // 점프 한 횟수 증가
                _jumpCount++;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 바닥에 닿았을 때 점프 횟수 초기화
            if (collision.gameObject.CompareTag("Ground"))
                _jumpCount = 0;
        }
    }
}
using Script._3D.UI;
using UnityEngine;

namespace Script._3D.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public float speed; // 이동속도
        public Vector3 targetPosition; // 이동 할 목표 좌표
        public GameObject player;

        private float _targetX;
        private float _targetZ;
        private float _playerX;
        private float _playerZ;
        private float _distanceX;
        private float _distanceZ;

        private bool _isMoveComplete;

        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            targetPosition = transform.position;

            _targetX = targetPosition.x;
            _targetZ = targetPosition.z;
            _playerX = player.transform.position.x;
            _playerZ = player.transform.position.z;
            _distanceX = Mathf.Abs(_targetX - _playerX);
            _distanceZ = Mathf.Abs(_targetZ - _playerZ);

            Debug.Log("distanceX: " + _distanceX + ", distanceZ: " + _distanceZ);

            _isMoveComplete = _distanceX < _distanceZ;
        }

        private void Update()
        {
            // 목적지에 근접하면 이동을 멈춤
            if (IsAtTargetPosition()) return;
            // 플레이어 이동

            if (_distanceX < _distanceZ)
            {
                if (!_isMoveComplete)
                {
                    Vector3 direction = _targetX > _playerX ? Vector3.right.normalized : Vector3.left.normalized;
                    transform.Translate(direction * (Time.deltaTime * speed), Space.World);
                    if (Mathf.Abs(_playerX - _targetX) < 0.2f)
                    {
                        _isMoveComplete = true;
                    }
                }
                else
                {
                    Vector3 direction = _targetZ > _playerZ ? Vector3.forward.normalized : Vector3.back.normalized;
                    transform.Translate(direction * (Time.deltaTime * speed), Space.World);
                    if (Mathf.Abs(_playerZ - _targetZ) < 0.2f)
                    {
                        _isMoveComplete = false;
                    }
                }
            }
            else
            {
                if (_isMoveComplete)
                {
                    Vector3 direction = _targetZ > _playerZ ? Vector3.forward.normalized : Vector3.back.normalized;
                    transform.Translate(direction * (Time.deltaTime * speed), Space.World);
                    if (Mathf.Abs(_playerZ - _targetZ) < 0.2f)
                    {
                        _isMoveComplete = false;
                    }
                }
                else
                {
                    Vector3 direction = _targetX > _playerX ? Vector3.right.normalized : Vector3.left.normalized;
                    transform.Translate(direction * (Time.deltaTime * speed), Space.World);
                    if (Mathf.Abs(_playerX - _targetX) < 0.2f)
                    {
                        _isMoveComplete = true;
                    }
                }
            }

            //Vector3 direction = (targetPosition - transform.position).normalized;
            //transform.Translate(direction * (Time.deltaTime * speed), Space.World);
        }

        // 목적지와의 거리 차이가 0.2f 이하인지 확인
        private bool IsAtTargetPosition()
        {
            return Vector3.Distance(transform.position, targetPosition) < 0.2f;
        }
    }
}
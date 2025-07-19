using UnityEngine;

namespace Aftab
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        float _forwardSpeed = 1.0f;
        [SerializeField]
        float _sideSpeed = 10.0f;

        float _laneDistance = 2.5f;

        int _currentLane = 1; // 0 = Left, 1 = Center, 2 = Right

        Vector3 _targetPosition = Vector3.zero;

        [SerializeField]
        bool _canMove = false;
        Rigidbody _rb;
        Vector3 _targetVelocity = Vector3.zero;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }
        void Update()
        {
            ManageInput();
            ManageSideWiseMovement();
        }

        void FixedUpdate()
        {
            ManageForwardMovement();
        }

        void ManageForwardMovement()
        {
            if (!_canMove) return;
            _targetVelocity.x = _rb.linearVelocity.x;
            _targetVelocity.y = _rb.linearVelocity.y;
            _targetVelocity.z = _forwardSpeed;
            _rb.linearVelocity = _targetVelocity;
        }

        void ManageInput()
        {
            // OLD INPUT SYSTEM: Will Replace with New Input System later
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                ChangeLane(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                ChangeLane(1);
            }
        }

        private void ChangeLane(int direction)
        {
            _currentLane = Mathf.Clamp(_currentLane + direction, 0, 2);
            float newX = (_currentLane - 1) * _laneDistance;
            _targetPosition = new Vector3(newX, transform.position.y, transform.position.z);
        }


        void ManageSideWiseMovement()
        {
            if (!_canMove) return;
            Vector3 newPosition = Vector3.Lerp(transform.position, 
                new Vector3(_targetPosition.x, transform.position.y, transform.position.z),
                Time.deltaTime * _sideSpeed);
            transform.position = newPosition;
        }
    }
}


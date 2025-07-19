using UnityEngine;

namespace Aftab
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        float _forwardSpeed = 1.0f;
        [SerializeField]
        float _sideSpeed = 1.0f;
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

        void ManageSideWiseMovement()
        {
            if (!_canMove) return;
        }
    }
}


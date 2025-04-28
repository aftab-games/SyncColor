using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float horizontalSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = 20f;

    private CharacterController _controller;
    private Vector3 _moveDirection;
    private MobileInputService _input;

    [Inject]
    public void Construct(MobileInputService input)
    {
        _input = input;
    }

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _input.OnSwipeUp += Jump;
        _input.OnDrag += MoveHorizontal;
    }

    void Update()
    {
        if (_controller.isGrounded)
        {
            _moveDirection.y = -1f;
        }

        _moveDirection.z = _input.IsTouching ? forwardSpeed : 0f;

        _moveDirection.y -= gravity * Time.deltaTime;
        _controller.Move(_moveDirection * Time.deltaTime);
    }

    void Jump()
    {
        if (_controller.isGrounded)
        {
            _moveDirection.y = jumpForce;
        }
    }

    void MoveHorizontal(Vector2 delta)
    {
        float h = delta.x * horizontalSpeed / Screen.width;
        _moveDirection.x = h;
    }
}

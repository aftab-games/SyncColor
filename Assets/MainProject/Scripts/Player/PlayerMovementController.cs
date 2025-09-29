using UnityEngine;
using UnityEngine.InputSystem;

namespace Aftab
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField]
        Rigidbody mainBodyRB;
        [SerializeField]
        LayerMask groundLayer;
        [SerializeField]
        float forwardSpeed = 20.0f;
        [SerializeField]
        float sideSpeed = 10.0f;
        [SerializeField]
        float jumpSpeed = 15.0f;
        float distancefromGround;
        bool canMove = false;
        float movementBound = 5.0f;
        Vector3 targetPosition = Vector3.zero;
        Vector3 targetVelocity = Vector3.zero;
        Vector3 pausedVelocity = Vector3.zero;
        Vector3 pausedAngularVelocity = Vector3.zero;

        GameManager gameManager;

        PlayerControls playerControls;
        InputAction moveAction;
        InputAction jumpAction;
        Vector2 moveInput;

        private void Awake()
        {
            canMove = false;
            playerControls = new PlayerControls();
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            gameManager.OnLevelStarted += OnLevelStarted;
            gameManager.OnLevelCompleted += OnLevelCompleted;
            gameManager.OnLevelFailed += OnLevelFailed;
            gameManager.OnActivateColorGate += OnPausePlayerMovement;
            //gameManager.OnChooseColorOption += ResumePlayerMovement; //TODO: Need to solve this
            gameManager.OnDeactivateColorGateWithoutChoose += OnResumePlayerMovement;
            distancefromGround = mainBodyRB.GetComponent<Collider>().bounds.extents.y + 0.1f;
            moveAction = playerControls.Player.Move;
            jumpAction = playerControls.Player.Jump;
            moveAction.Enable();
            jumpAction.Enable();
            moveAction.performed += OnMoveActionPerformed;
            jumpAction.performed += OnJumpActionPerformed;
        }

        private void OnDisable()
        {
            gameManager.OnLevelStarted -= OnLevelStarted;
            gameManager.OnLevelCompleted += OnLevelCompleted;
            gameManager.OnLevelFailed += OnLevelFailed;
            gameManager.OnActivateColorGate -= OnPausePlayerMovement;
            //gameManager.OnChooseColorOption -= ResumePlayerMovement; // ResumeMovementAndSetColor; //TODO: Need to solve this
            gameManager.OnDeactivateColorGateWithoutChoose -= OnResumePlayerMovement;
            moveAction.performed -= OnMoveActionPerformed;
            jumpAction.performed -= OnJumpActionPerformed;
            moveAction.Disable();
            jumpAction.Disable();
        }

        #region Event Actions
        private void OnLevelStarted()
        {
            canMove = true;
        }

        private void OnLevelCompleted()
        {
            canMove = false;
        }

        private void OnLevelFailed()
        {
            canMove = false;
        }

        private void OnPausePlayerMovement()
        {
            //TODO: Store current state, Physics states. These state will be usefull when resume the movement
            //Halt the movement
            canMove = false;
            pausedVelocity = mainBodyRB.linearVelocity;
            pausedAngularVelocity = mainBodyRB.angularVelocity;
            mainBodyRB.isKinematic = true;
        }

        private void OnResumePlayerMovement()
        {
            //TODO: Resume player movement using stored physcis states
            mainBodyRB.isKinematic = false;
            mainBodyRB.linearVelocity = pausedVelocity;
            mainBodyRB.angularVelocity = pausedAngularVelocity;
            canMove = true;
        }

        void OnJumpActionPerformed(InputAction.CallbackContext ctx)
        {
            Debug.Log(" Jump Action: " + ctx.ToString());
            if (!canMove) return;
            if (!IsGrounded()) return;
            mainBodyRB.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }

        void OnMoveActionPerformed(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();
            Debug.Log("Move Input: " + moveInput.ToString());
        }

        #endregion

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            
        }

        void ManageInput()
        {
            if (!canMove) return;

        }

        bool IsGrounded() => Physics.Raycast(mainBodyRB.transform.position, Vector3.down, distancefromGround, groundLayer);

    }
}

using System;
using TMPro;
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
        bool canJump = false;
        float boundX = 5.0f;
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
            moveInput = Vector3.zero;
            playerControls = new PlayerControls();
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            gameManager.OnLevelStarted += OnLevelStarted;
            gameManager.OnLevelCompleted += OnLevelCompleted;
            gameManager.OnLevelFailed += OnLevelFailed;
            gameManager.OnActivateColorGate += PausePlayerMovement;
            gameManager.OnChooseColorOption += ManageOnChooseColorOption;
            gameManager.OnDeactivateColorGateWithoutChoose += ResumePlayerMovement;
            distancefromGround = mainBodyRB.GetComponent<Collider>().bounds.extents.y + 0.1f;
            moveAction = playerControls.Player.Move;
            jumpAction = playerControls.Player.Jump;
            moveAction.Enable();
            jumpAction.Enable();
            moveAction.performed += OnMoveActionPerformed;
            moveAction.canceled += OnMoveActionCancelled;
            jumpAction.performed += OnJumpActionPerformed;
        }

        private void OnDisable()
        {
            gameManager.OnLevelStarted -= OnLevelStarted;
            gameManager.OnLevelCompleted += OnLevelCompleted;
            gameManager.OnLevelFailed += OnLevelFailed;
            gameManager.OnActivateColorGate -= PausePlayerMovement;
            gameManager.OnChooseColorOption -= ManageOnChooseColorOption;
            gameManager.OnDeactivateColorGateWithoutChoose -= ResumePlayerMovement;
            moveAction.performed -= OnMoveActionPerformed;
            moveAction.canceled -= OnMoveActionCancelled;
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

        private void PausePlayerMovement()
        {
            //TODO: Store current state, Physics states. These state will be usefull when resume the movement
            //Halt the movement
            canMove = false;
            pausedVelocity = mainBodyRB.linearVelocity;
            pausedAngularVelocity = mainBodyRB.angularVelocity;
            mainBodyRB.isKinematic = true;
        }

        private void ResumePlayerMovement()
        {
            //TODO: Resume player movement using stored physcis states
            mainBodyRB.isKinematic = false;
            mainBodyRB.linearVelocity = pausedVelocity;
            mainBodyRB.angularVelocity = pausedAngularVelocity;
            canMove = true;
        }

        private void ManageOnChooseColorOption(GateKeySO gateKeySo)
        {
            ResumePlayerMovement();
        }

        private void OnJumpActionPerformed(InputAction.CallbackContext ctx)
        {
            if (!canMove) return;
            if (!IsGrounded()) return;
            canJump = true;
        }

        private void OnMoveActionPerformed(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();
            //Debug.Log("Move Input: " + moveInput.ToString());
        }

        private void OnMoveActionCancelled(InputAction.CallbackContext ctx)
        {
            moveInput = Vector2.zero;
        }

        #endregion

        private void FixedUpdate()
        {
            ManageMovement();
        }
        float currVel;
        private void ManageMovement()
        {
            if (!canMove) return;
            
            ManageJumpVel();
            targetVelocity.z = forwardSpeed;

            targetVelocity.x = mainBodyRB.linearVelocity.x;
            mainBodyRB.linearVelocity = targetVelocity;
            SideWiseMovementMove();

            void ManageJumpVel()
            {
                //TODO: Need to make it more snappy. i.e: When going down, we can add force to down in forcemode.accelerate
                if (canJump)
                {
                    targetVelocity.y = jumpSpeed;
                    canJump = false;
                }
                else targetVelocity.y = mainBodyRB.linearVelocity.y;
            }

            void SideWiseMovementMove()
            {
                if (!CanMoveSideWise()) return;

                Vector3 trgtPos = mainBodyRB.position;
                
                trgtPos.x = Mathf.Clamp(Mathf.SmoothDamp(trgtPos.x, trgtPos.x + moveInput.x * 5,
                    ref currVel, 0.2f, 100, Time.fixedDeltaTime), -boundX, boundX); //trgtPos.x = Mathf.Clamp(Mathf.MoveTowards(trgtPos.x, trgtPos.x + moveInput.x, sideSpeed * Time.fixedDeltaTime), -boundX, boundX);

                //Debug.Log($"CurrVel: {currVel} , MaxSpeed: {sideSpeed}");
                mainBodyRB.MovePosition(trgtPos);
            }

            bool CanMoveSideWise()
            {
                float currentPositionX = mainBodyRB.position.x;
                //return moveInput.x > 0 && currentPositionX >= (boundX - 0.5f) || moveInput.x < 0 && currentPositionX <= -(boundX - 0.5f) ? false : true;
                if (moveInput.x > 0 && currentPositionX >= boundX)
                {
                    return false;
                }
                else if(moveInput.x < 0 && currentPositionX <= -boundX)
                {
                    return false;
                }
                return true;
            }

            void SideWiseMovementVelAndMovePos()
            {
                if (CanMoveSideWise())
                {
                    float inputXClamped = Mathf.Clamp(moveInput.x * Time.fixedDeltaTime, -1.0f, 1.0f);
                    targetVelocity.x = inputXClamped * sideSpeed;
                    mainBodyRB.linearVelocity = targetVelocity;
                }
                else
                {
                    Vector3 clampedPosition = mainBodyRB.position;
                    clampedPosition.x = Mathf.Clamp(clampedPosition.x, -boundX, boundX);
                    mainBodyRB.MovePosition(clampedPosition);
                }
            }
        }

        private bool IsGrounded() => Physics.Raycast(mainBodyRB.transform.position, Vector3.down, distancefromGround, groundLayer);

    }
}

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
        float jumpSpeed = 15.0f;
        float distancefromGround;
        bool canMove = false;
        bool canJump = false;
        float boundX = 5.0f;
        float currVelRef;
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
            gameManager.OnLevelStarted += MakeCanMoveTrue;
            gameManager.OnLevelCompleted += OnLevelCompleted;
            gameManager.OnPlayerFallingDown += MakeCanMoveFasle;
            gameManager.OnPlayerHit += StopBallMovement;
            gameManager.OnActivateColorGate += PausePlayerMovement;
            gameManager.OnChooseColorOption += ManageOnChooseColorOption;
            gameManager.OnDeactivateColorGateWithoutChoose += ResumePlayerMovement;
            distancefromGround = mainBodyRB.GetComponent<Collider>().bounds.extents.y + 0.1f;
            //Input System
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
            gameManager.OnLevelStarted -= MakeCanMoveTrue;
            gameManager.OnLevelCompleted -= OnLevelCompleted;
            gameManager.OnPlayerFallingDown -= MakeCanMoveFasle;
            gameManager.OnPlayerHit -= StopBallMovement;
            gameManager.OnActivateColorGate -= PausePlayerMovement;
            gameManager.OnChooseColorOption -= ManageOnChooseColorOption;
            gameManager.OnDeactivateColorGateWithoutChoose -= ResumePlayerMovement;
            //Input System
            moveAction.performed -= OnMoveActionPerformed;
            moveAction.canceled -= OnMoveActionCancelled;
            jumpAction.performed -= OnJumpActionPerformed;
            moveAction.Disable();
            jumpAction.Disable();
        }

        #region Event Actions
        
        private void OnLevelCompleted()
        {
            StopBallMovement();
        }


        private void MakeCanMoveTrue()
        {
            canMove = true;
        }

        private void MakeCanMoveFasle()
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
            mainBodyRB.linearVelocity = Vector3.zero;
            mainBodyRB.angularVelocity = Vector3.zero;
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

        private void StopBallMovement() //At the level end
        {
            MakeCanMoveFasle();
            mainBodyRB.linearVelocity = Vector3.zero;
            mainBodyRB.angularVelocity = Vector3.zero;
        }

        private void ManageOnChooseColorOption(GateKeySO gateKeySo) //TODO: This need to be solved. Player movement doesn't need to know about GateKeySO
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
                
                trgtPos.x = Mathf.Clamp(Mathf.SmoothDamp(mainBodyRB.position.x, trgtPos.x + moveInput.x,
                    ref currVelRef, 0.075f, 100, Time.fixedDeltaTime), -boundX, boundX); //trgtPos.x = Mathf.Clamp(Mathf.MoveTowards(trgtPos.x, trgtPos.x + moveInput.x, sideSpeed * Time.fixedDeltaTime), -boundX, boundX);

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

        }

        private bool IsGrounded() => Physics.Raycast(mainBodyRB.transform.position, Vector3.down, distancefromGround, groundLayer);

    }
}

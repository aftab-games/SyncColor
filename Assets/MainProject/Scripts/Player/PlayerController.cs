using DG.Tweening.Core.Easing;
using UnityEngine;

namespace Aftab
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }
        [Header("Movement Settings")]
        [SerializeField]
        Rigidbody mainBodyRB;
        [SerializeField]
        LayerMask groundLayer;
        [SerializeField]
        float forwardSpeed = 10.0f;
        [SerializeField]
        float sideSpeed = 10.0f;
        [SerializeField]
        float jumpSpeed = 10.0f;
        float distancefromGround;
        Transform ballMainBodyTr;
        bool canMove = false;
        bool canJump = false;
        float laneDistance = 4.5f;
        int currentLane = 1; // 0 = Left, 1 = Center, 2 = Right
        Vector3 targetPosition = Vector3.zero;
        Vector3 targetVelocity = Vector3.zero;
        Vector3 pausedVelocity = Vector3.zero;
        Vector3 pausedAngularVelocity = Vector3.zero;

        [Header("Color Settings")]
        [SerializeField]
        Renderer playerRenderer;
        [SerializeField]
        TrailRenderer trailRenderer;
        [SerializeField]
        Color playerColor;

        GameManager gameManager;
        BallBreakingHandler ballBreakingHandler;


        void Awake()
        {
            Instance = this;
            canMove = false;
            canJump = false;
            ballMainBodyTr = mainBodyRB.transform;
            SetBodyAndTrailColor(playerColor);
            ballBreakingHandler = GetComponent<BallBreakingHandler>();
        }

        void Start()
        {
            gameManager = GameManager.Instance;
            gameManager.OnLevelStarted += ManageOnGameStarted;
            gameManager.OnActivateColorGate += PausePlayerMovement;
            gameManager.OnChooseColorOption += ResumeMovementAndSetColor;
            gameManager.OnDeactivateColorGateWithoutChoose += ResumePlayerMovement;
            distancefromGround = mainBodyRB.GetComponent<Collider>().bounds.extents.y + 0.1f;
        }

        void OnDisable()
        {
            gameManager.OnLevelStarted -= ManageOnGameStarted;
            gameManager.OnActivateColorGate -= PausePlayerMovement;
            gameManager.OnChooseColorOption -= ResumeMovementAndSetColor;
            gameManager.OnDeactivateColorGateWithoutChoose -= ResumePlayerMovement;
        }

        void ManageOnGameStarted()
        {
            canMove = true;
        }

        void Update()
        {
            if(gameManager.IsLevelCompleted || gameManager.IsLevelCompleted) return;
            ManageInput(); //This needs to be moved to InputManager script
        }

        void FixedUpdate()
        {
            ManageForwardAndUpMovement();
            ManageSideWiseMovement();
        }

        void ManageInput()
        {
            // OLD INPUT SYSTEM: Will Replace with New Input System later
            // TODO: Polish the movement system
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                ChangeLane(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                ChangeLane(1);
            }

            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                //Jump
                canJump = true;
            }

        }

        void ResumeMovementAndSetColor(GateKeySO gateKeySO)
        {
            ResumePlayerMovement();
            SetBodyAndTrailColor(gateKeySO.GateColor);
        }

        void PausePlayerMovement()
        {
            //TODO: Store current state, Physics states. These state will be usefull when resume the movement
            //Halt the movement
            canMove = false;
            pausedVelocity = mainBodyRB.linearVelocity;
            pausedAngularVelocity = mainBodyRB.angularVelocity;
            mainBodyRB.isKinematic = true;
        }

        void ResumePlayerMovement()
        {
            //TODO: Resume player movement using stored physcis states
            mainBodyRB.isKinematic = false;
            mainBodyRB.linearVelocity = pausedVelocity;
            mainBodyRB.angularVelocity = pausedAngularVelocity;
            canMove = true;
        }

        void ManageForwardAndUpMovement()
        {
            if (!canMove) return;
            targetVelocity.x = mainBodyRB.linearVelocity.x;
            targetVelocity.y = mainBodyRB.linearVelocity.y;
            targetVelocity.z = forwardSpeed;
            mainBodyRB.linearVelocity = targetVelocity;
            ManageJump();
            void ManageJump()
            {
                //Check can jump
                if (!canJump) return;
                //Check is grounded
                if (!IsGrounded()) return;
                mainBodyRB.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                canJump = false;
                bool IsGrounded() => Physics.Raycast(ballMainBodyTr.position, Vector3.down, distancefromGround, groundLayer);
            }
        }

        void ChangeLane(int direction)
        {
            currentLane = Mathf.Clamp(currentLane + direction, 0, 2);
            float targetXPos = (currentLane - 1) * laneDistance;
            targetPosition = new Vector3(targetXPos, ballMainBodyTr.position.y, ballMainBodyTr.position.z);
        }


        void ManageSideWiseMovement()
        {
            //TODO: Polish the movement system.
            //As the object is moving forward by rigidboy, it's sidewise movement should be determined by rigidbody too
            if (!canMove) return;
            Vector3 newPosition = new Vector3(targetPosition.x, ballMainBodyTr.position.y, ballMainBodyTr.position.z);
            ballMainBodyTr.position = Vector3.Lerp(ballMainBodyTr.position, newPosition, Time.deltaTime * sideSpeed);
        }

        void SetBodyAndTrailColor(Color newColor)
        {
            playerColor = newColor;
            if (playerRenderer != null)
            {
                //main body
                //playerRenderer.material.color = playerColor;
                MaterialPropertyBlock mpBlock = new MaterialPropertyBlock();
                playerRenderer.GetPropertyBlock(mpBlock);
                mpBlock.SetColor("_BaseColor", playerColor);
                playerRenderer.SetPropertyBlock(mpBlock);
                //Trail
                //trailRenderer.material.color = playerColor;
                //trailRenderer.material.SetColor("_BaseColor", playerColor);
                trailRenderer.GetPropertyBlock(mpBlock);
                mpBlock.SetColor("_BaseColor", playerColor);
                trailRenderer.SetPropertyBlock(mpBlock);
            }
        }

        
        void BallAnimationAtStart()
        {
            //Have a nice and smooth animation at the start.
            //Animaiton will continue before player start the game.
        }

        public void BreakBodyIntoPieces()
        {
            ballBreakingHandler.ManageBallBreak();
        }

        public void ManageFallingDown()
        {
            canMove = false;
        }

        public void StopBallMovement()
        {
            canMove = false;
            mainBodyRB.linearVelocity = Vector3.zero;
            mainBodyRB.angularVelocity = Vector3.zero;
        }

        public Color GetPlayerColor()
        {
            return playerColor;
        }

        public Transform GetBallTransform()
        {
            return ballMainBodyTr;
        }
    }
}
/*
bool IsColorMatch(Color a, Color b)
{
    float tolerance = 0.1f;
    return Mathf.Abs(a.r - b.r) < tolerance &&
           Mathf.Abs(a.g - b.g) < tolerance &&
           Mathf.Abs(a.b - b.b) < tolerance;
}
*/
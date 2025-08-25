using System.Diagnostics.Contracts;
using UnityEngine;

namespace Aftab
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }
        [Header("Movement Settings")]
        [SerializeField]
        float forwardSpeed = 1.0f;
        [SerializeField]
        float sideSpeed = 10.0f;
        [SerializeField]
        Rigidbody mainBodyRB;
        Transform ballMainBodyTr;
        bool canMove = false;
        float laneDistance = 2.5f;
        int currentLane = 1; // 0 = Left, 1 = Center, 2 = Right
        Vector3 targetPosition = Vector3.zero;
        Vector3 targetVelocity = Vector3.zero;

        [Header("Color Settings")]
        [SerializeField]
        Renderer playerRenderer;
        [SerializeField]
        Color playerColor;
        bool isMatchedColor = false;
        Color matchedColor = Color.white;
        
        void Awake()
        {
            Instance = this;
            canMove = false;
            ballMainBodyTr = mainBodyRB.transform;
            SetPlayerColor(playerColor);
        }

        void Start()
        {
            GameManager.Instance.OnLevelStarted += ManageOnGameStarted;
        }

        void OnDisable()
        {
            GameManager.Instance.OnLevelStarted -= ManageOnGameStarted;
        }

        void ManageOnGameStarted()
        {
            canMove = true;
        }

        void Update()
        {
            if(GameManager.Instance.IsLevelCompleted || GameManager.Instance.IsLevelCompleted) return;
            ManageInput(); //This needs to be moved to InputManager script
            ManageSideWiseMovement();
        }

        void FixedUpdate()
        {
            ManageForwardMovement();
        }

        void ManageForwardMovement()
        {
            if (!canMove) return;
            targetVelocity.x = mainBodyRB.linearVelocity.x;
            targetVelocity.y = mainBodyRB.linearVelocity.y;
            targetVelocity.z = forwardSpeed;
            mainBodyRB.linearVelocity = targetVelocity;
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

            if(GameManager.Instance.IsAllowedInputForGateOpening)
            {
                isMatchedColor = false;
                matchedColor = Color.white;
                if (Input.GetKeyDown(KeyCode.R))
                {
                    isMatchedColor = GameManager.Instance.CheckInputWithCurrentGateColorCode(GateColorCode.R, out matchedColor);
                }
                else if(Input.GetKeyDown(KeyCode.G))
                {
                    isMatchedColor = GameManager.Instance.CheckInputWithCurrentGateColorCode(GateColorCode.G,out matchedColor);
                }
                else if(Input.GetKeyDown(KeyCode.B))
                {
                    isMatchedColor = GameManager.Instance.CheckInputWithCurrentGateColorCode(GateColorCode.B, out matchedColor);
                }

                if(isMatchedColor)
                {
                    SetPlayerColor(matchedColor);
                }
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

        void SetPlayerColor(Color newColor)
        {
            playerColor = newColor;
            if (playerRenderer != null)
            {
                playerRenderer.material.color = playerColor;
            }
        }

        

        void BallAnimationAtStart()
        {
            //Have a nice and smooth animation at the start.
            //Animaiton will continue before player start the game.
        }

        public void BreakBodyIntoPieces()
        {
            if(TryGetComponent<BallBreakingHandler>(out BallBreakingHandler ballBreakingHandler))
            {
                ballBreakingHandler.ManageBallBreak();
            }
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
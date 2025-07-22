using UnityEngine;

namespace Aftab
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField]
        bool _canMove = false;
        [SerializeField]
        float _forwardSpeed = 1.0f;
        [SerializeField]
        float _sideSpeed = 10.0f;
        float _laneDistance = 2.5f;
        int _currentLane = 1; // 0 = Left, 1 = Center, 2 = Right
        Vector3 _targetPosition = Vector3.zero;
        Rigidbody _rb;
        Vector3 _targetVelocity = Vector3.zero;

        [Header("Color Settings")]
        [SerializeField]
        Renderer playerRenderer;
        [SerializeField]
        Color playerColor;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            SetPlayerColor(playerColor);
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

            if(GameManager.Instance.IsAllowedInputForGateOpening)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    GameManager.Instance.CheckInputWithCurrentGateColorCode(GateColorCode.R);
                }
                else if(Input.GetKeyDown(KeyCode.G))
                {
                    GameManager.Instance.CheckInputWithCurrentGateColorCode(GateColorCode.G);
                }
                else if(Input.GetKeyDown(KeyCode.B))
                {
                    GameManager.Instance.CheckInputWithCurrentGateColorCode(GateColorCode.B);
                }
            }
        }

        void ChangeLane(int direction)
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

        void BreakBodyIntoPieces()
        {
            //Play an Fx
            //Deactivate current body
            //Activate pieces
            //Activate rigidbody of broken pieces
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Gate")) //Activator is trigger
            {
                if(other.TryGetComponent<ColorGate>(out ColorGate colorGate))
                {
                    Debug.Log("Gate Triggered");
                    colorGate.ActivateGate();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            //TODO: Add obstacles in the path. Will implement Jump to avoid obstacles which can be avoided by jumping
            //Will have a screen shake
            if (collision.gameObject.CompareTag("Gate")) //Main gate is collider
            {
                Debug.Log("Gate Hit");
                BreakBodyIntoPieces();
                //Send a message to Game manager
                GameManager.Instance.PlayerCollideWithGate();
            }
        }

        public Color GetPlayerColor()
        {
            return playerColor;
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
using Unity.VisualScripting;
using UnityEngine;

namespace Aftab
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        //TODO: This class can inherit from PlayerController. Don't know if it's good practice or not. Need to find out
        PlayerController playerController;

        bool shouldHandleCollision = false;

        void Start()
        {
            shouldHandleCollision = false;
            playerController = PlayerController.Instance;
            GameManager.Instance.OnLevelStarted += ManageOnLevelStarted;
            GameManager.Instance.OnLevelCompleted += ManageOnLevelCompleted;
            GameManager.Instance.OnLevelFailed += ManageOnLevelFailed;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnLevelStarted -= ManageOnLevelStarted;
            GameManager.Instance.OnLevelCompleted -= ManageOnLevelCompleted;
            GameManager.Instance.OnLevelFailed -= ManageOnLevelFailed;
        }

        void ManageOnLevelStarted()
        {
            shouldHandleCollision = true;
        }

        void ManageOnLevelCompleted()
        {
            shouldHandleCollision = false;
        }

        void ManageOnLevelFailed()
        {
            shouldHandleCollision = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!shouldHandleCollision) return;
            if (other.gameObject.CompareTag("Gate")) //Activator is trigger
            {
                if (other.TryGetComponent<ColorGate>(out ColorGate colorGate))
                {
                    Debug.Log("Gate triggered");
                    colorGate.ActivateGate();
                }
            }
            else if (other.gameObject.CompareTag("LevelEnd"))
            {
                Debug.Log("Level End triggered");
                playerController.StopBallMovement();
                GameManager.Instance.PlayerReachedAtLevelEnd();
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (!shouldHandleCollision) return;
            //TODO: Add obstacles in the path. Will implement Jump to avoid obstacles which can be avoided by jumping
            //Will have a screen shake
            if (collision.gameObject.CompareTag("Gate")) //Main gate is collider
            {
                Debug.Log("Gate Hit");
                playerController.StopBallMovement();
                playerController.BreakBodyIntoPieces();
                //Send a message to Game manager
                GameManager.Instance.PlayerCollideWithGate();
            }
        }
    }
}

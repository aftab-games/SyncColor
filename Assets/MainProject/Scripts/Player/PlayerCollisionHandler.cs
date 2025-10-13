using UnityEngine;

namespace Aftab
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        bool shouldHandleCollision = false;

        void Start()
        {
            shouldHandleCollision = false;
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
            if (other.gameObject.CompareTag("Gate"))
            {
                if (other.TryGetComponent<ColorGate>(out ColorGate colorGate))
                {
                    colorGate.ActivateGate();
                }
            }
            else if (other.gameObject.CompareTag("LevelEnd"))
            {
                GameManager.Instance.PlayerReachedAtLevelEnd();
            }
            else if (other.gameObject.CompareTag("FallingDownTrigger"))
            {
                GameManager.Instance.PlayerFallingDown();
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (!shouldHandleCollision) return;
            if (collision.gameObject.CompareTag("Gate"))
            {
                GameManager.Instance.PlayerCollideWithGate();
            }
            else if (collision.gameObject.CompareTag("Obstacles"))
            {
                GameManager.Instance.PlayerCollideWithObstacle();
            }
        }
    }
}

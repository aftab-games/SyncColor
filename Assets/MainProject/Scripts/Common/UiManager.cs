using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Aftab
{
    public class UiManager : MonoBehaviour
    {
        [Header("Settings:")]
        [SerializeField]
        GameObject lvlCompletePanelGO;
        [SerializeField]
        GameObject lvlFailedPanelGO;
        [SerializeField]
        Button startGameBtn, nextLvlBtn, retryBtn;
        GameManager gameManager;

        void Start()
        {
            gameManager = GameManager.Instance;
            gameManager.OnLvlCompleted += ActivateLvlCompletedPanel;
            gameManager.OnLvlFailed += ActivateLvlFailedPanel;
            startGameBtn.onClick.AddListener(ManageTappedStartGameBtn);
            nextLvlBtn.onClick.AddListener (ManageTappedNextLvlBtn);
            retryBtn.onClick.AddListener (ManageTappedRetryBtn);
        }

        void OnDisable()
        {
            gameManager.OnLvlCompleted -= ActivateLvlCompletedPanel;
            gameManager.OnLvlFailed -= ActivateLvlFailedPanel;
            startGameBtn.onClick.RemoveAllListeners();
            nextLvlBtn.onClick.RemoveAllListeners();
            retryBtn.onClick.RemoveAllListeners();
        }
        void ManageTappedStartGameBtn()
        {
            startGameBtn.gameObject.SetActive(false);
            GameManager.Instance.ManageGameStarted();
        }
        void ManageTappedNextLvlBtn()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //TODO: Reloading the scene at this moment.Need to fix this
        }
        void ManageTappedRetryBtn()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        void ActivateLvlCompletedPanel()
        {
            lvlCompletePanelGO.SetActive(true);
        }

        void ActivateLvlFailedPanel()
        {
            lvlFailedPanelGO.SetActive(true);
        }
    }
}

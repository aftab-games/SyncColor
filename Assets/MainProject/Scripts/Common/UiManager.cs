using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using UITKButton = UnityEngine.UIElements.Button;

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

        //UI Tool Kit
        [SerializeField]
        UIDocument uiDocument;
        VisualElement root;

        VisualElement startPanel;
        VisualElement completePanel;
        VisualElement failedPanel;

        UITKButton startButton;
        UITKButton nextButton;
        UITKButton retryButton;

        void Awake()
        {
            //UI Tool Kit
            root = uiDocument.rootVisualElement;

            // Query panels
            startPanel = root.Q<VisualElement>("StartPanel");
            completePanel = root.Q<VisualElement>("LevelCompletePanel");
            failedPanel = root.Q<VisualElement>("LevelFailedPanel");

            // Query buttons
            startButton = root.Q<UITKButton>("StartButton");
            nextButton = root.Q<UITKButton>("NextButton");
            retryButton = root.Q<UITKButton>("RetryButton");
        }

        void Start()
        {
            gameManager = GameManager.Instance;
            gameManager.OnLevelCompleted += () => ActivateLvlCompletedPanel();
            gameManager.OnLevelFailed += ActivateLvlFailedPanel;
            //startGameBtn.onClick.AddListener(ManageTappedStartGameBtn); //Disabling for UIToolKit
            //nextLvlBtn.onClick.AddListener (ManageTappedNextLvlBtn); //Disabling for UIToolKit
            //retryBtn.onClick.AddListener (ManageTappedRetryBtn); //Disabling for UIToolKit

            //UI Tool Kit
            gameManager.OnLevelStarted += () => HidePanel(startPanel);
            gameManager.OnLevelCompleted += () => ShowPanel(completePanel);
            gameManager.OnLevelFailed += () => ShowPanel(failedPanel);

            // Button callbacks
            startButton.clicked += () => ManageTappedStartGameBtn();
            nextButton.clicked += () => ManageTappedNextLvlBtn();
            retryButton.clicked += () => ManageTappedRetryBtn();
            //
            // Initial state
            ShowPanel(startPanel);
        }

        void OnDisable()
        {
            gameManager.OnLevelCompleted -= ActivateLvlCompletedPanel;
            gameManager.OnLevelFailed -= ActivateLvlFailedPanel;
            //startGameBtn.onClick.RemoveAllListeners(); //Disabling for UIToolKit
            //nextLvlBtn.onClick.RemoveAllListeners(); //Disabling for UIToolKit
            //retryBtn.onClick.RemoveAllListeners(); //Disabling for UIToolKit

            //UI Tool Kit
            gameManager.OnLevelStarted -= () => HidePanel(startPanel);
            gameManager.OnLevelCompleted -= () => ShowPanel(completePanel);
            gameManager.OnLevelFailed -= () => ShowPanel(failedPanel);
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
            _ = DelayedActivateLvlCompletedPanel();
            //Implement awaitable
            //TODO: Implemeent Unitask
            async Awaitable DelayedActivateLvlCompletedPanel()
            {
                await Awaitable.WaitForSecondsAsync(1);
                lvlCompletePanelGO.SetActive(true);
            }

        }

        void ActivateLvlFailedPanel()
        {
            _ = DelayedActivateLvlFailedPanel();
            //Implement awaitable
            //TODO: Implemeent Unitask
            async Awaitable DelayedActivateLvlFailedPanel()
            {
                await Awaitable.WaitForSecondsAsync(1);
                lvlFailedPanelGO.SetActive(true);
            }
        }

        //UI Tool Kit
        // ========= Fade Helpers =========
        void ShowPanel(VisualElement panel)
        {
            panel.style.display = DisplayStyle.Flex;
            panel.style.opacity = 0f;

            // Delay 1 frame before fade (UI Toolkit quirk)
            _ = FadeIn(panel);
        }

        void HidePanel(VisualElement panel)
        {
            _ = FadeOut(panel);
        }

        async Awaitable FadeIn(VisualElement panel)
        {
            await Awaitable.NextFrameAsync(); //Wait 1 frame
            panel.style.opacity = 1f;
        }

        async Awaitable FadeOut(VisualElement panel)
        {
            panel.style.opacity = 0f;
            await Awaitable.WaitForSecondsAsync(0.4f); // matches USS transition-duration
            panel.style.display = DisplayStyle.None;
        }
    }
}

using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
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
        GameManager gameManager; //This needs to be optimized following SOLID - Should be decoupled

        [Header("Settings:")]
        [SerializeField]
        GameObject lvlCompletePanelGO;
        [SerializeField]
        GameObject lvlFailedPanelGO;
        [SerializeField]
        Button startGameBtn, nextLvlBtn, retryBtn;
        [Header("Level No Related:")]
        [SerializeField]
        GameObject levelBoxGO;
        [SerializeField]
        UnityEngine.UI.Image levelProgressFillImage;
        [SerializeField]
        TextMeshProUGUI levelNoTMP;

        //Gate opener button //This should be in separate script for SRP of SOLID and Buttons should be modular
        [Header("Gate Opener Related:")]
        [SerializeField]
        GameObject gateOpenerGO;
        [SerializeField]
        Transform gateOpenerBoxTr;
        [SerializeField]
        List<Button> gateOpenerButtons = new List<Button>();
        [SerializeField]
        [Tooltip("Should aligned with GateOpeningButtons serial. Y=Yellow, G=Green, B=Blue. Button's color will be assigned from this.")]
        List<GateKeySO> gateKeySOs = new List<GateKeySO>();
        
        [Header("Gate Opener Countdown:")]
        [SerializeField]
        GameObject gateOpenerCountDownGO;
        [SerializeField]
        TextMeshProUGUI gateOpenerCountDownTMP;
        [SerializeField]
        Transform gateOpenerCountDownTr;
        bool isGateOpenerActivated = false;
        CancellationTokenSource cncltnTknSrc;

        //UI Tool Kit
        [Header("UIToolKit Settings:")]
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

            //Hide panels
            startPanel.style.display = DisplayStyle.None;
            completePanel.style.display = DisplayStyle.None;
            failedPanel.style.display = DisplayStyle.None;
        }

        void Start()
        {
            gameManager = GameManager.Instance;
            gameManager.OnActivateColorGate += () => ManageActivateGateBox();
            AssignGateOpeningButtonsColor();
            //TODO: Manage Button click of color gate button 
            for (int i = 0; i < gateOpenerButtons.Count; i++)
            {
                int tempIndex = i;
                gateOpenerButtons[i].onClick.AddListener(()=> ManageGateOpenerButtonClick(tempIndex));
            }
            //gameManager.OnLevelCompleted += () => ActivateLvlCompletedPanel();
            //gameManager.OnLevelFailed += ActivateLvlFailedPanel;
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
            // Initial state
            //root.style.display = DisplayStyle.Flex;
            ShowPanel(startPanel);
            //End of UiToolKit
        }

        void OnDisable()
        {
            for (int i = 0; i < gateOpenerButtons.Count; i++)
            {
                int tempIndex = i;
                gateOpenerButtons[i].onClick.RemoveListener(() => ManageGateOpenerButtonClick(tempIndex));
            }
            //gameManager.OnLevelCompleted -= ActivateLvlCompletedPanel;
            //gameManager.OnLevelFailed -= ActivateLvlFailedPanel;
            //startGameBtn.onClick.RemoveAllListeners(); //Disabling for UIToolKit
            //nextLvlBtn.onClick.RemoveAllListeners(); //Disabling for UIToolKit
            //retryBtn.onClick.RemoveAllListeners(); //Disabling for UIToolKit
            gameManager.OnActivateColorGate -= () => ManageActivateGateBox();
            //UI Tool Kit
            gameManager.OnLevelStarted -= () => HidePanel(startPanel);
            gameManager.OnLevelCompleted -= () => ShowPanel(completePanel);
            gameManager.OnLevelFailed -= () => ShowPanel(failedPanel);
            // Button callbacks
            startButton.clicked -= () => ManageTappedStartGameBtn();
            nextButton.clicked -= () => ManageTappedNextLvlBtn();
            retryButton.clicked -= () => ManageTappedRetryBtn();
        }

        void ManageActivateGateBox()
        {
            //Activate panel
            isGateOpenerActivated = true;
            gateOpenerBoxTr.localScale = Vector3.zero;
            float countDownDuration = 3f;
            gateOpenerCountDownTMP.text = countDownDuration.ToString();
            gateOpenerGO.SetActive(true);
            gateOpenerBoxTr.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            foreach (Button button in gateOpenerButtons)
            {
                button.interactable = true;
            }
            //Activate countdown with resetting for 3 seconds
            //ActivateCountDown();
            //Activate countdown in a async await or coroutine eventually in UniTask
            StartCountDown();
            async void StartCountDown()
            {
                cncltnTknSrc = new CancellationTokenSource();

                try
                {
                    while (countDownDuration > 0)
                    {
                        gateOpenerCountDownTMP.text = Mathf.CeilToInt(countDownDuration).ToString();
                        gateOpenerCountDownTr.DOPunchScale(Vector3.one * 0.3f, 0.3f);
                        await Awaitable.WaitForSecondsAsync(1f, cncltnTknSrc.Token); // Wait for 1 second
                        countDownDuration--;
                    }
                }
                catch (OperationCanceledException)
                {
                    Debug.Log("Countdown cancelled.");
                    //gateOpenerCountDownTMP.text = "!";
                }
                finally
                {
                    if (isGateOpenerActivated)
                    {
                        //Deactivate box
                        ManageDeactivateGateBox();
                        //Resume player movement
                        gameManager.ManageDeactivatedColorGateWithoutChooseOption();
                    }
                    cncltnTknSrc.Dispose();
                }
            }
        }

        void ManageDeactivateGateBox()
        {
            isGateOpenerActivated = false;
            //Disable all the buttons
            foreach (Button button in gateOpenerButtons)
            {
                button.interactable = false;
            }
            cncltnTknSrc?.Cancel();
            gateOpenerBoxTr.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(()=> gateOpenerGO.SetActive(false));
        }

        void AssignGateOpeningButtonsColor()
        {
            if (gateKeySOs == null || gateOpenerButtons == null)
            {
                Debug.LogError("GateKeySOs, GateOpeningButton or both are null.");
                return;
            }
            if (gateKeySOs.Count == 0 || gateOpenerButtons.Count == 0)
            {
                Debug.LogError("GateKeySOs, GateOpeningButton or both are empty.");
                return;
            }
            if (gateKeySOs.Count != gateOpenerButtons.Count)
            {
                Debug.LogError("GateKeySOs and GateOpeningButtons are not same in quantity.");
                return;
            }

            //Assigning colors of buttons. It can be done by getting UI image of those buttons
            ColorBlock colorBlock = new ColorBlock();
            for (int i = 0; i < gateOpenerButtons.Count; i++)
            {
                colorBlock = gateOpenerButtons[i].colors;
                colorBlock.normalColor = gateKeySOs[i].GateColor;
                gateOpenerButtons[i].colors = colorBlock;
            }
        }

        void ManageGateOpenerButtonClick(int buttonIndex)
        {
            Debug.Log("Button Index: " + buttonIndex);
            ManageDeactivateGateBox();
            gameManager.ManageChooseGateColorOption(gateKeySOs[buttonIndex]);
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

/*void ActivateCountDown()
{

    
     * public async UniTaskVoid StartCountdown() // Use UniTaskVoid for methods not returning a value
{
cts = new CancellationTokenSource();
float currentTime = countdownDuration;

try
{
while (currentTime > 0)
{
    countdownText.text = Mathf.CeilToInt(currentTime).ToString();
    await UniTask.Delay(System.TimeSpan.FromSeconds(1), cancellationToken: cts.Token); // Wait for 1 second
    currentTime--;
}
countdownText.text = "GO!";
}
catch (OperationCanceledException)
{
Debug.Log("Countdown cancelled.");
countdownText.text = "Cancelled";
}
finally
{
cts.Dispose();
}
}

public void CancelCountdown()
{
cts?.Cancel();
}
     
}
*/
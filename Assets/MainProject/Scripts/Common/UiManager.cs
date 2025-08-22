using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Aftab
{
    public class UiManager : MonoBehaviour
    {
        [Header("Settings:")]
        [SerializeField]
        GameObject lvlCompletePanelGO, lvlFailedPanelGO;
        [SerializeField]
        Button startGameBtn, nextLvlBtn, retryBtn;

        public static UiManager Instance {  get; private set; }

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            startGameBtn.onClick.AddListener(ManageTappedStartGameBtn);
            nextLvlBtn.onClick.AddListener (ManageTappedNextLvlBtn);
            retryBtn.onClick.AddListener (ManageTappedRetryBtn);
        }

        void OnDisable()
        {
            startGameBtn.onClick.RemoveAllListeners();
            nextLvlBtn.onClick.RemoveAllListeners();
            retryBtn.onClick.RemoveAllListeners();
        }
        void ManageTappedStartGameBtn()
        {

        }
        void ManageTappedNextLvlBtn()
        {

        }
        void ManageTappedRetryBtn()
        {

        }
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aftab
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance {  get; private set; } //We can use zenject for clean architecture

        ColorGate currentColorGate = null; //We can use zenject for clean architecture

        public bool IsLevelStarted { get; private set; }
        public bool IsLevelCompleted { get; private set; }
        public bool IsLevelFailed { get; private set; }
        public Transform BallMainBodyTr { get; set; } //TODO: This should be solved. Not sure if ball body transform should be Get from the GameManager

        public event Action OnLevelStarted, OnLevelCompleted, OnLevelFailed, OnPlayerHit, OnPlayerFallingDown;
        public event Action OnActivateColorGate;
        public event Action<GateKeySO> OnChooseColorOption;
        public event Action OnDeactivateColorGateWithoutChoose;

        void Awake()
        {
            Instance = this;
            Application.targetFrameRate = 60;
            //Input.multiTouchEnabled = false; // it won't work in new input system environment
            IsLevelStarted = false;
        }

        public void ManageGameStarted()
        {
            OnLevelStarted?.Invoke();
            IsLevelStarted = true;
        }

        public void ManageActivateGateOpening(bool allow, ColorGate colorGate)
        {
            currentColorGate = colorGate;
            OnActivateColorGate?.Invoke();
        }

        public void ManageChooseGateColorOption(GateKeySO gateKeySO) //This sould be called from UiManager
        {
            if(currentColorGate.ThisGateKeySO.GateKeyCode == gateKeySO.GateKeyCode)
            {
                OpenCurrentGate();
                ResetGateOpeningAndCurrentColorGate();
            }
            OnChooseColorOption?.Invoke(gateKeySO);
        }

        public void ManageDeactivatedColorGateWithoutChooseOption()
        {
            OnDeactivateColorGateWithoutChoose?.Invoke();
        }

        void OpenCurrentGate()
        {
            //Disable instruction UI
            //Give an imoji effect along with a vfx effect, may be
            if(currentColorGate != null) currentColorGate.OpenGate(); //TODO: Need to solve this. I mean decouple this. And implement Zenject
            
        }

        void ResetGateOpeningAndCurrentColorGate()
        {
            currentColorGate = null;
        }

        public void PlayerCollideWithGate()
        {
            //At this moment, Level failed will be called
            ManageLevelFailed();
            OnPlayerHit?.Invoke();
        }

        public void PlayerCollideWithObstacle()
        {
            //At this moment, Level failed will be called
            ManageLevelFailed();
            OnPlayerHit?.Invoke();
        }

        public void PlayerFallingDown()
        {
            //At this moment, Level failed will be called
            ManageLevelFailed();
            OnPlayerFallingDown?.Invoke();
        }

        public void PlayerReachedAtLevelEnd()
        {
            ManageLevelComplete();
        }

        void ManageLevelComplete()
        {
            IsLevelCompleted = true;
            OnLevelCompleted?.Invoke();
        }

        void ManageLevelFailed()
        {
            IsLevelFailed = true;
            //Camshake
            //Destroy ball. Make ball broken in pieces
            OnLevelFailed?.Invoke();
        }
    }
}

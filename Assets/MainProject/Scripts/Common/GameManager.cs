using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aftab
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance {  get; private set; } //We can use zenject for clean architecture

        ColorGate _currentColorGate = null; //We can use zenject for clean architecture

        public bool IsLevelStarted { get; private set; }
        public bool IsLevelCompleted { get; private set; }
        public bool IsLevelFailed { get; private set; }
        public bool IsAllowedInputForGateOpening { get; private set; }

        public event Action OnLevelStarted, OnLevelCompleted, OnLevelFailed;

        void Awake()
        {
            Instance = this;
            IsLevelStarted = false;
        }

        public void ManageGameStarted()
        {
            OnLevelStarted?.Invoke();
            IsLevelStarted = true;
        }

        public void ManageAllowingGateOpening(bool allow, ColorGate colorGate)
        {
            IsAllowedInputForGateOpening = allow;
            _currentColorGate = colorGate;
            //If allow then 
            //Now, Show a UI message to press key (R for red, G for green, and B for blue)
            //Or, Open a UI panel with button showing different color.
        }

        public bool CheckInputWithCurrentGateColorCode(GateColorCode colorCode, out Color matchedGateColor)
        {
            matchedGateColor = Color.white;
            bool isMatched = false;
            if (colorCode == _currentColorGate.ThisGateColorCode)
            {
                //Matched the color code
                matchedGateColor = _currentColorGate.ThisGateColor;
                OpenCurrentGate();
                ResetGateOpeningAndCurrentColorGate();
                isMatched = true;
            }
            return isMatched;
        }

        void OpenCurrentGate()
        {
            //Disable instruction UI
            //Give an imoji effect along with a vfx effect, may be
            if(_currentColorGate != null) _currentColorGate.OpenGate(); //TODO: Need to solve this. I mean decouple this. And implement Zenject
            
        }

        void ResetGateOpeningAndCurrentColorGate()
        {
            IsAllowedInputForGateOpening = false;
            _currentColorGate = null;
        }

        public void PlayerCollideWithGate()
        {
            //At this moment, Level failed will be called
            ManageLevelFailed();
        }

        public void PlayerReachedAtLevelEnd()
        {
            ManageLevelComplete();
        }

        void ManageLevelComplete()
        {
            IsLevelCompleted = true;
            IsAllowedInputForGateOpening = false;
            OnLevelCompleted?.Invoke();
        }

        void ManageLevelFailed()
        {
            IsLevelFailed = true;
            IsAllowedInputForGateOpening = false;
            //Camshake
            //Destroy ball. Make ball broken in pieces
            OnLevelFailed?.Invoke();
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

namespace Aftab
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance {  get; private set; } //We can use zenject for clean architecture

        bool _isLevelCompleted = false;
        bool _isLevelFailed = false;
        bool _isAllowedInputForGateOpening = false;
        ColorGate _currentColorGate = null; //We can use zenject for clean architecture

        public bool IsLevelCompleted => _isLevelCompleted;
        public bool IsLevelFailed => _isLevelFailed;
        public bool IsAllowedInputForGateOpening => _isAllowedInputForGateOpening;

        void Awake()
        {
            Instance = this;
        }

        

        public void ManageAllowingGateOpening(bool allow, ColorGate colorGate)
        {
            _isAllowedInputForGateOpening = allow;
            _currentColorGate = colorGate;
            //If allow then 
            //Now, Show a UI message to press key (R for red, G for green, and B for blue)
            //Or, Open a UI panel with button showing different color.
        }

        public void CheckInputWithCurrentGateColorCode(GateColorCode colorCode)
        {
            if(colorCode == _currentColorGate.ThisGateColorCode)
            {
                //Matched the color code
                OpenCurrentGate();
            }
        }

        void OpenCurrentGate()
        {
            //Disable instruction UI
            //Give an imoji effect along with a vfx effect, may be
            if(_currentColorGate != null) _currentColorGate.OpenGate(); //TODO: Need to solve this. I mean decouple this. And implement Zenject
            _isAllowedInputForGateOpening = false;
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
            _isLevelCompleted = true;
            _isAllowedInputForGateOpening = false;
            //Open an UI panel
            //Open next button
        }

        void ManageLevelFailed()
        {
            _isLevelFailed = true;
            _isAllowedInputForGateOpening = false;
            //Open an UI panel
            //Open retry button
        }
    }
}

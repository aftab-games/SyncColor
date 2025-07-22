using UnityEngine;

namespace Aftab
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; } //Need to utilize Zenject to inject necessary items
        void Awake()
        {
            Instance = this;
        }

    }
}

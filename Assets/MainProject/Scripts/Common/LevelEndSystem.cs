using UnityEngine;

namespace Aftab
{
    public class LevelEndSystem : MonoBehaviour
    {
        public static LevelEndSystem Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}

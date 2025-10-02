using UnityEngine;

namespace Aftab
{
    public enum GateKey //TODO: This needs to be solved. May be we do not need this extra enum, instead we can use SO info
    {
        //Y = Yellow, G = Green, B = Blue
        Y = 0, G = 1, B = 2
    }
    [CreateAssetMenu(fileName = "GateKeySO", menuName = "Scriptable Objects/GateKeySO")]
    public class GateKeySO : ScriptableObject
    {
        [SerializeField]
        GateKey key;
        [SerializeField]
        Color color;

        public GateKey GateKeyCode => key;
        public Color GateColor => color;
    }
}

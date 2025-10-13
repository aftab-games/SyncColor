using UnityEngine;

namespace Aftab
{
    public class PlayerVisualHandler : MonoBehaviour
    {
        [Header("Settings:")]
        [SerializeField]
        Transform ballMainBodyTr;
        [SerializeField]
        Renderer playerRenderer;
        [SerializeField]
        GateKeySO initialGateKeySO;
        Color bodyCurrentColor;

        private void Start()
        {
            GameManager.Instance.BallMainBodyTr = ballMainBodyTr;
            GameManager.Instance.OnChooseColorOption += OnChooseColorOption;
            SetBodyColor(initialGateKeySO.GateColor);
        }

        private void OnDisable()
        {
            GameManager.Instance.OnChooseColorOption -= OnChooseColorOption;
        }

        private void OnChooseColorOption(GateKeySO gateKeySO)
        {
            SetBodyColor(gateKeySO.GateColor);
        }

        private void SetBodyColor(Color color)
        {
            if (playerRenderer != null)
            {
                MaterialPropertyBlock mpBlock = new MaterialPropertyBlock();
                playerRenderer.GetPropertyBlock(mpBlock); //playerRenderer.GetPropertyBlock(mpBlock, 0);
                mpBlock.SetColor("_BaseColor", color);
                playerRenderer.SetPropertyBlock(mpBlock);
                bodyCurrentColor = color;
            }
        }

        public Color GetPlayerColor() //This should be get from visual handler
        {
            return bodyCurrentColor;
        }
    }
}

/*
bool IsColorMatch(Color a, Color b)
{
    float tolerance = 0.1f;
    return Mathf.Abs(a.r - b.r) < tolerance &&
           Mathf.Abs(a.g - b.g) < tolerance &&
           Mathf.Abs(a.b - b.b) < tolerance;
}
*/
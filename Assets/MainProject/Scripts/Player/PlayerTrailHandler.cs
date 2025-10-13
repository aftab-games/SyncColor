using UnityEngine;

namespace Aftab
{
    public class PlayerTrailHandler : MonoBehaviour
    {
        [Header("Settings:")]
        [SerializeField]
        TrailRenderer trailRenderer;
        [SerializeField]
        GateKeySO initialGateKeySO;

        private void Start()
        {
            GameManager.Instance.OnChooseColorOption += OnChooseColorOption;
            SetTrailColor(initialGateKeySO.GateColor);
        }

        private void OnDisable()
        {
            GameManager.Instance.OnChooseColorOption -= OnChooseColorOption;
        }

        void OnChooseColorOption(GateKeySO gateKeySO)
        {
            SetTrailColor(gateKeySO.GateColor);
        }

        public void SetTrailColor(Color color)
        {
            if(trailRenderer == null) return;
            MaterialPropertyBlock mpBlock = new MaterialPropertyBlock();
            trailRenderer.GetPropertyBlock(mpBlock);
            mpBlock.SetColor("_BaseColor", color);
            trailRenderer.SetPropertyBlock(mpBlock);
        }
    }
}

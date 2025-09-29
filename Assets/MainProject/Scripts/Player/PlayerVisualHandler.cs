using UnityEngine;

namespace Aftab
{
    public class PlayerVisualHandler : MonoBehaviour
    {
        [SerializeField]
        Renderer playerRenderer;

        public void SetBodyColor(Color color)
        {
            if (playerRenderer != null)
            {
                MaterialPropertyBlock mpBlock = new MaterialPropertyBlock();
                playerRenderer.GetPropertyBlock(mpBlock);
                mpBlock.SetColor("_BaseColor", color);
                playerRenderer.SetPropertyBlock(mpBlock);
            }
        }
    }
}

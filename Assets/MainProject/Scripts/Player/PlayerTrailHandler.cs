using UnityEngine;

namespace Aftab
{
    public class PlayerTrailHandler : MonoBehaviour
    {
        [SerializeField]
        TrailRenderer trailRenderer;
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

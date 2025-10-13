using UnityEngine;
using DG.Tweening;

namespace Aftab
{
    public class ColorGate : MonoBehaviour //We can create this as modular by implementing interface
    {
        [SerializeField]
        GateKeySO gateKeySO;
        [SerializeField]
        Renderer gateRenderer;
        [SerializeField]
        Collider gateActivatorTrigger;
        [SerializeField]
        Collider gateCollider;
        [SerializeField]
        Transform gateTransform;

        public GateKeySO ThisGateKeySO => gateKeySO;

        void Start()
        {
            gateRenderer.material.color = gateKeySO.GateColor; //TODO: Need to optimize it by accesing shader property to change color of the material not instantiated material
        }

        public void ActivateGate()
        {
            gateActivatorTrigger.enabled = false;
            //Send a message to game manager that a color gate is activated of certain color.
            GameManager.Instance.ManageActivateGateOpening(true, this);
            
        }

        public void OpenGate()
        {
            gateCollider.enabled = false;
            //Have a tween animation to open the gate
            gateTransform.DOLocalMoveY(gateTransform.localPosition.y + 30, 1f).SetEase(Ease.OutSine)
                .OnComplete(()=> gameObject.SetActive(false));
            //_gateTransform.DOScale(0, 1f).SetEase(Ease.OutSine).OnComplete(()=> gameObject.SetActive(false));

        }
    }
}

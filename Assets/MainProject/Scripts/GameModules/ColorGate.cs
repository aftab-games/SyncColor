using UnityEngine;
using DG.Tweening;

namespace Aftab
{
    public enum GateColorCode
    {
        R =0, G = 1, B = 2
    }
    public class ColorGate : MonoBehaviour
    {
        [SerializeField]
        Color _gateColor;
        [SerializeField]
        GateColorCode _gateColorCode;
        [SerializeField]
        Renderer _renderer;
        [SerializeField]
        Collider _gateActivatorTrigger;
        [SerializeField]
        Collider _gateCollider;
        [SerializeField]
        Transform _gateTransform;

        public GateColorCode ThisGateColorCode => _gateColorCode;
        public Color ThisGateColor => _gateColor;

        void Start()
        {
            _renderer.material.color = _gateColor; //TODO: Need to optimize it by accesing shader property to change color of the material not instantiated material
        }

        public void ActivateGate()
        {
            _gateActivatorTrigger.enabled = false;
            //Send a message to game manager that a color gate is activated of certain color.
            GameManager.Instance.ManageAllowingGateOpening(true, this);
            
        }

        public void OpenGate()
        {
            _gateCollider.enabled = false;
            //Have a tween animation to open the gate
            _gateTransform.DOLocalMoveY(_gateTransform.localPosition.y + 30, 1f).SetEase(Ease.OutSine)
                .OnComplete(()=> gameObject.SetActive(false));
            //_gateTransform.DOScale(0, 1f).SetEase(Ease.OutSine).OnComplete(()=> gameObject.SetActive(false));

        }
    }
}

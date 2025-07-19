using UnityEngine;
using DG.Tweening;

namespace Aftab
{
    public class TweenTest : MonoBehaviour
    {
        [SerializeField]
        Transform _HammerTr;
        [SerializeField]
        bool _ChangeInLocalPos = true;
        [SerializeField]
        float _FinalYPos;
        [SerializeField]
        float _InitialDelay, _TimeForInitialToFinal = 0, _TimeForFinalToInitial = 0, _TimeForMiddleGap = 0, _TimeForInitialGap = 0;
        [SerializeField]
        Ease _FinalPosOutEase;
        float _InitialYPos;
        void Start()
        {
            Invoke(nameof(AnimateTr), _InitialDelay);
        }

        void AnimateTr()
        {
            Sequence animSequence = DOTween.Sequence().SetId(this);
            if (_ChangeInLocalPos)
            {
                _InitialYPos = _HammerTr.localPosition.y;
                animSequence.Append(_HammerTr.DOLocalMoveY(_FinalYPos, _TimeForInitialToFinal).SetDelay(_TimeForInitialGap).SetEase(_FinalPosOutEase));
                animSequence.Append(_HammerTr.DOLocalMoveY(_InitialYPos, _TimeForFinalToInitial).SetDelay(_TimeForMiddleGap));
            }
            else
            {
                _InitialYPos = _HammerTr.position.y;
                animSequence.Append(_HammerTr.DOMoveY(_FinalYPos, _TimeForInitialToFinal).SetDelay(_TimeForInitialGap).SetEase(_FinalPosOutEase));
                animSequence.Append(_HammerTr.DOMoveY(_InitialYPos, _TimeForFinalToInitial).SetDelay(_TimeForMiddleGap));
            }
            animSequence.SetLoops(-1);
        }

        public void StopAnimation()
        {
            DOTween.Kill(this);
        }
    }
}

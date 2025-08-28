using DG.Tweening;
using UnityEngine;

namespace Aftab
{
    public class DTAnimation : MonoBehaviour
    {
        enum AnimationType
        {
            Move,
            Rotate,
            ContinousRotate,
            MoveRotate,
            MoveContinousRotate
        }

        [Header("Settings:")]
        [SerializeField]
        bool _shouldAnimateAtStart = false;
        [SerializeField]
        AnimationType _animationType;
        [SerializeField]
        Transform _toAnimateTr;
        [SerializeField]
        Vector3 _finalPos;
        [SerializeField]
        Vector3 _finalRot;

        [SerializeField]
        float _posTime, _rotTime;
        [SerializeField]
        Ease _posEase;
        [SerializeField]
        Ease _rotEase;
        [SerializeField]
        Vector3 _continousRotAxis;
        [SerializeField]
        float _timeForContRot = 2;

        Vector3 _initialPos, _initilRot;

        void Start()
        {
            if (_shouldAnimateAtStart) Animate();
        }

        void OnDisable()
        {
            KillAnimation();
        }
        void Animate()
        {
            _initialPos = _toAnimateTr.localPosition;
            _initilRot = _toAnimateTr.localEulerAngles;
            //Cont rot data
            Vector3 contRotData = Vector3.zero;
            float rotVal = 360;
            if (_continousRotAxis.x != 0)
            {
                if (_continousRotAxis.x > 0) rotVal = 360;
                else rotVal = -360;
                contRotData = new Vector3(rotVal, _initilRot.y, _initilRot.z);
            }
            else if (_continousRotAxis.y != 0)
            {
                if (_continousRotAxis.y > 0) rotVal = 360;
                else rotVal = -360;
                contRotData = new Vector3(_initilRot.x, rotVal, _initilRot.z);
            }
            else if (_continousRotAxis.z != 0)
            {
                if (_continousRotAxis.z > 0) rotVal = 360;
                else rotVal = -360;
                contRotData = new Vector3(_initilRot.x, _initilRot.y, rotVal);
            }
            //End of cont rot data

            switch (_animationType)
            {
                case AnimationType.Move:
                    _toAnimateTr.DOLocalMove(_finalPos, _posTime).SetEase(_posEase).SetLoops(-1, LoopType.Yoyo);
                    break;
                case AnimationType.Rotate:
                    _toAnimateTr.DOLocalRotateQuaternion(Quaternion.Euler(_finalRot), _rotTime).SetEase(_rotEase).SetLoops(-1, LoopType.Yoyo);
                    break;
                case AnimationType.ContinousRotate:
                    _toAnimateTr.DOLocalRotate(contRotData, _timeForContRot, RotateMode.FastBeyond360).SetEase(_rotEase).SetLoops(-1, LoopType.Incremental);
                    break;
                case AnimationType.MoveRotate:
                    _toAnimateTr.DOLocalMove(_finalPos, _posTime).SetEase(_posEase).SetLoops(-1, LoopType.Yoyo);
                    _toAnimateTr.DOLocalRotateQuaternion(Quaternion.Euler(_finalRot), _rotTime).SetEase(_rotEase).SetLoops(-1, LoopType.Yoyo);
                    break;
                case AnimationType.MoveContinousRotate:
                    _toAnimateTr.DOLocalMove(_finalPos, _posTime).SetEase(_posEase).SetLoops(-1, LoopType.Yoyo);
                    _toAnimateTr.DOLocalRotate(contRotData, _timeForContRot, RotateMode.FastBeyond360).SetEase(_rotEase).SetLoops(-1, LoopType.Incremental);
                    break;
            }
        }

        public void KillAnimation()
        {
            DOTween.Kill(this);
        }

    }
}

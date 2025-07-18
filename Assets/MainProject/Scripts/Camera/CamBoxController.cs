using UnityEngine;
//using DG.Tweening;

namespace Aftab
{
    public class CamBoxController : MonoBehaviour
    {
        public static CamBoxController Instance;

        //[SerializeField]
        Transform _trToFollow;
        //[SerializeField]
        Transform _trToLook;
        [SerializeField]
        [Tooltip("This vector will specify the follow axis. 1 equivalent true, 0 equivalent false.")]
        Vector3 followAxis = Vector3.one;
        [SerializeField]
        [Tooltip("This vector will specify the follow speed per axis")]
        Vector3 followSpeed = Vector3.one;
        [SerializeField]
        [Tooltip("This float will specify the look speed on y axis")]
        float lookSpeed = 1;
        [SerializeField]
        CameraFollowSmoothType cameraFollowSmoothType;

        [SerializeField]
        CameraLookSmoothType _cameraLookSmoothType;

        Quaternion targetRotation;
        Vector3 targetPos = Vector3.zero;
        Vector3 initialFollowSpeed;
        float initialLookSpeed;

        public bool canFollow = false, canLook = false;

        void Awake() => Instance = this;

        void Start()
        {
            initialFollowSpeed = followSpeed;
            initialLookSpeed = lookSpeed;

            //GameController.Instance.OnGameStarted += ManageOnGameStarted;
            //GameController.Instance.OnLevelCompleted += ManageOnLevelCompleted;
            //GameController.Instance.OnLevelFailed += ManageOnLevelFailed;
        }

        void OnDisable()
        {
            //GameController.Instance.OnGameStarted -= ManageOnGameStarted;
            //GameController.Instance.OnLevelCompleted -= ManageOnLevelCompleted;
            //GameController.Instance.OnLevelFailed -= ManageOnLevelFailed;
        }

        void ManageOnGameStarted()
        {
            //canFollow = true;
            //canLook = true;
        }
        void ManageOnLevelCompleted()
        {
            canFollow = false;
            canLook = false;
        }
        void ManageOnLevelFailed()
        {
            canFollow = false;
            canLook = false;
        }

        void LateUpdate()
        {
            FollowObject();
            LookObject();
        }

        void FollowObject()
        {
            if (_trToFollow == null) return;
            if (!canFollow) return;
            if (cameraFollowSmoothType == CameraFollowSmoothType.SmoothFollow)
            {
                if (followAxis.x == 1) targetPos.x = Mathf.Lerp(transform.position.x, _trToFollow.position.x, followSpeed.x * Time.deltaTime);
                else targetPos.x = transform.position.x;
                if (followAxis.y == 1) targetPos.y = Mathf.Lerp(transform.position.y, _trToFollow.position.y, followSpeed.y * Time.deltaTime);
                else if (followAxis.y == 2) targetPos.y = _trToFollow.position.y;
                else targetPos.y = transform.position.y;
                if (followAxis.z == 1) targetPos.z = Mathf.Lerp(transform.position.z, _trToFollow.position.z, followSpeed.z * Time.deltaTime);
                else targetPos.z = transform.position.z;

                transform.position = targetPos;
            }
            else if (cameraFollowSmoothType == CameraFollowSmoothType.DirectFollow)
            {
                targetPos = _trToFollow.position;
                transform.position = targetPos;
            }
        }

        void LookObject()
        {
            if (_trToLook == null) return;
            if (!canLook) return;
            if (_cameraLookSmoothType == CameraLookSmoothType.SmoothLook)
            {
                targetRotation = Quaternion.Euler(0, _trToLook.eulerAngles.y, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
            }
            else
            {
                targetRotation = Quaternion.Euler(0, _trToLook.eulerAngles.y, 0);
                transform.rotation = targetRotation;
            }
        }

        public void SetObjectToFollowAndLook(Transform trToFollow = null, Transform trToLook = null)
        {
            _trToFollow = trToFollow;
            _trToLook = trToLook;
        }

        public void ChangeFollowSpeed(Vector3 _targetSpeed, float _duration)
        {
            //DOTween.To(() => followSpeed, x => followSpeed = x, _targetSpeed, _duration);
        }

        public void ChangeFollowAxisToNormalized()
        {
            followAxis = Vector3.one;
        }
    }

    public enum CameraFollowSmoothType
    {
        SmoothFollow,
        DirectFollow
    }

    public enum CameraLookSmoothType
    {
        SmoothLook,
        DirectLook
    }
}


using UnityEngine;

namespace Aftab
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; private set; }
        public Camera MainCam { get; private set; }
        [SerializeField]
        bool shouldCallManageCameraRatio = true;
        const float longDeviceSizeMultiplier = 1.04f;
        const float _refSR = (float)1920 / 1080;
        float screenRatio;

        void Awake()
        {
            Instance = this;
            MainCam = GetComponent<Camera>();
            ManageCameraRatio();
        }

        /// <summary>
        /// This will manage the camera fov or orthographic size according to device screen ratio
        /// </summary>
        void ManageCameraRatio()
        {
            if (!shouldCallManageCameraRatio) return;
            screenRatio = (float)Screen.height / Screen.width;
            //If screen ratio is less than 1.78f
            //if(screenRatio <= 1.78f) return;
            //If this camera is not orthographic
            if (!MainCam.orthographic) MainCam.fieldOfView = (MainCam.fieldOfView * screenRatio * longDeviceSizeMultiplier) / _refSR;
            else MainCam.orthographicSize = (MainCam.orthographicSize / _refSR) * screenRatio;
        }
    }
}


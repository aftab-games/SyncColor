using System.Collections;
using UnityEngine;

namespace Aftab
{
    public class CameraShake : MonoBehaviour
    {
        [Header("Settings:")]
        [SerializeField]
        float duration;
        [SerializeField]
        float magnitude;
        Coroutine runningCoroutine;

        void Start()
        {
            GameManager.Instance.OnLvlFailed += ManageShakeCamera;
        }
        void OnDisable()
        {
            GameManager.Instance.OnLvlFailed -= ManageShakeCamera;
        }

        void ManageShakeCamera()
        {
            ShakeTheCamera(duration, magnitude);
        }

        void ShakeTheCamera(float _duration, float _magnitude)
        {
            if (runningCoroutine != null) StopCoroutine(runningCoroutine);
            runningCoroutine = StartCoroutine(Shake(_duration, _magnitude));
        }

        IEnumerator Shake(float _duration, float _magnitude)
        {
            Vector3 originalPos = transform.localPosition;

            float elapsedTime = 0.0f;

            while (elapsedTime < _duration)
            {
                float x = originalPos.x + Random.Range(-1f, 1f) * _magnitude;
                float y = originalPos.y + Random.Range(-1f, 1f) * _magnitude;
                float z = originalPos.z + Random.Range(-1f, 1f) * _magnitude;

                transform.localPosition = new Vector3(x, y, originalPos.z);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalPos;
        }
    }
}


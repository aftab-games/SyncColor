using System.Collections;
using UnityEngine;

namespace Aftab
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance { get; private set; }

        Coroutine runningCoroutine;

        void Awake() => Instance = this;

        public void ShakeTheCamera(float _duration, float _magnitude)
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


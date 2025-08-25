using UnityEngine;

namespace Aftab
{
    public class FxManager : MonoBehaviour
    {
        [Header("Settings:")]
        [SerializeField]
        ParticleSystem winFx;
        [SerializeField]
        ParticleSystem ballBreakFx;
        void Start()
        {
            GameManager.Instance.OnLevelCompleted += ManageLevelCompleted;
            GameManager.Instance.OnLevelFailed += PlayBallBreakFx;
        }

        void OnDisable()
        {
            GameManager.Instance.OnLevelCompleted -= ManageLevelCompleted;
            GameManager.Instance.OnLevelFailed -= PlayBallBreakFx;
        }
        void ManageLevelCompleted()
        {
            winFx.transform.position = LevelEndSystem.Instance.transform.position + new Vector3(0, 8, 5);
            winFx.gameObject.SetActive(true);
            Invoke(nameof(PlayWinFx), 0.1f);
        }

        void PlayWinFx()
        {
            winFx.Play();
        }

        void PlayBallBreakFx()
        {
            ballBreakFx.transform.position = PlayerController.Instance.GetBallTransform().position;
            ballBreakFx.gameObject.SetActive(true);
            ballBreakFx.Play();
        }
    }
}

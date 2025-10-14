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
        [SerializeField]
        ParticleSystem soulEscapeFx;
        void Start()
        {
            GameManager.Instance.OnLevelCompleted += ManageLevelCompleted;
            GameManager.Instance.OnPlayerHit += PlayBallBreakFx;
            GameManager.Instance.OnPlayerHit += PlaySoulEscapeFx;
            GameManager.Instance.OnPlayerFallingDown += PlaySoulEscapeFx;
        }

        void OnDisable()
        {
            GameManager.Instance.OnLevelCompleted -= ManageLevelCompleted;
            GameManager.Instance.OnPlayerHit -= PlayBallBreakFx;
            GameManager.Instance.OnPlayerHit -= PlaySoulEscapeFx;
            GameManager.Instance.OnPlayerFallingDown -= PlaySoulEscapeFx;
        }
        void ManageLevelCompleted()
        {
            winFx.transform.position = LevelEndSystem.Instance.transform.position + Vector3.up * 13;
            winFx.gameObject.SetActive(true);
            Invoke(nameof(PlayWinFx), 0.1f);
        }

        void PlayWinFx()
        {
            winFx.Play();
        }

        void PlayBallBreakFx()
        {
            ballBreakFx.transform.position = GameManager.Instance.BallMainBodyTr.position;
            ballBreakFx.gameObject.SetActive(true);
            ballBreakFx.Play();
        }

        void PlaySoulEscapeFx()
        {
            soulEscapeFx.transform.position = GameManager.Instance.BallMainBodyTr.position;
            soulEscapeFx.gameObject.SetActive(true);
            soulEscapeFx.Play();
        }
    }
}

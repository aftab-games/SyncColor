using UnityEngine;

namespace Aftab
{
    public class BallBreakingHandler : MonoBehaviour
    {
        [Header("Settings:")]
        [SerializeField]
        GameObject brokenPiecesParentGO;
        [SerializeField]
        PlayerVisualHandler playerVisualHandler;

        private void Start()
        {
            GameManager.Instance.OnPlayerHit += ManageBallBreak;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnPlayerHit += ManageBallBreak;
        }

        void ManageBallBreak()
        {
            Transform brokenBallTr = Instantiate(brokenPiecesParentGO, transform).transform;
            brokenBallTr.SetParent(transform);
            Transform ballMainBodyTr = GameManager.Instance.BallMainBodyTr;
            
            brokenBallTr.localPosition = ballMainBodyTr.localPosition;
            brokenBallTr.localRotation = ballMainBodyTr.localRotation;
            brokenBallTr.localScale = ballMainBodyTr.localScale;
            Color currentBallColor = playerVisualHandler.GetPlayerColor();
            ballMainBodyTr.gameObject.SetActive(false);
            foreach (Transform childTr in brokenBallTr)
            {
                //Debug.Log(childTr.name);
                MeshRenderer meshRenderer = childTr.GetComponent<MeshRenderer>();
                meshRenderer.material.color = currentBallColor;
                Rigidbody rigidbody = childTr.GetComponent<Rigidbody>();
                //rigidbody.AddExplosionForce(5000, brokenBallTr.position, 20, 20, ForceMode.VelocityChange);
                rigidbody.linearVelocity = new Vector3(Random.Range(-5, 5), Random.Range(3, 5), Random.Range(-5, 5));
            }
        }
    }
}

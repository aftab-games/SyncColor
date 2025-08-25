using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Aftab
{
    public class BallBreakingHandler : MonoBehaviour
    {
        [Header("Settings:")]
        [SerializeField]
        GameObject brokenPiecesParentGO;

        public void ManageBallBreak()
        {
            //TODO: Instantiate prefab
            Transform brokenBallTr = Instantiate(brokenPiecesParentGO, transform).transform;
            //TODO: Place inside the player controller. I mean make the instantiated object as a child of this object
            brokenBallTr.SetParent(transform);
            Transform ballMainBodyTr = PlayerController.Instance.GetBallTransform();
            
            brokenBallTr.localPosition = ballMainBodyTr.localPosition;
            brokenBallTr.localRotation = ballMainBodyTr.localRotation;
            brokenBallTr.localScale = ballMainBodyTr.localScale;
            //TODO: Get currentballColor
            Color currentBallColor = PlayerController.Instance.GetPlayerColor();
            //TODO: Deactivate Ball Main Transform
            ballMainBodyTr.gameObject.SetActive(false);
            //TODO: Get all the mesh renderer
            //TODO: Assign current ball color to all the renderer
            //TODO: Get all the rigidbody insided
            //TODO: Give an explosion force or custom force
            //List<Transform> childTransforms = brokenBallTr.GetComponentsInChildren<Transform>().ToList<Transform>();
            foreach (Transform childTr in brokenBallTr)
            {
                Debug.Log(childTr.name);
                MeshRenderer meshRenderer = childTr.GetComponent<MeshRenderer>();
                meshRenderer.material.color = currentBallColor;
                Rigidbody rigidbody = childTr.GetComponent<Rigidbody>();
                //rigidbody.AddExplosionForce(5000, brokenBallTr.position, 20, 20, ForceMode.VelocityChange);
                rigidbody.linearVelocity = new Vector3(Random.Range(-5, 5), Random.Range(3, 5), Random.Range(-5, 5));
            }
        }
    }
}

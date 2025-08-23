using UnityEngine;

namespace Aftab
{
    public class BallBreakingHandler : MonoBehaviour
    {
        [Header("Settings:")]
        [SerializeField]
        GameObject brokenPiecesParentGO;

        void ManageBallBreak()
        {
            //TODO: Instantiate prefab
            //TODO: Place inside the player controller. I mean make the instantiated object as a child of this object
            //TODO: Get all the mesh renderer
            //TODO: Assign current ball color to all the renderer
            //TODO: Get all the rigidbody insided
            //TODO: Give an explosion force or custom force
        }
    }
}

using UnityEngine;

namespace Aftab
{
    public class SetObjectToCamFollow : MonoBehaviour
    {
        [SerializeField]
        Transform trToFollow;
        void Start()
        {
            CamBoxController.Instance.SetObjectToFollowAndLook(trToFollow, trToFollow);
        }
    }
}


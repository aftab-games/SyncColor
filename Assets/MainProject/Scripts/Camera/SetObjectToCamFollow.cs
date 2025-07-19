using UnityEngine;

namespace Aftab
{
    public class SetObjectToCamFollow : MonoBehaviour
    {
        //Attach this script to the object which will be followed by camera
        void Start()
        {
            CamBoxController.Instance.SetObjectToFollowAndLook(transform, transform);
        }
    }
}


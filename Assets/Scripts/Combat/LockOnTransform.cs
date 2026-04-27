using UnityEngine;
using NaughtyAttributes;

namespace Group1 {
    public class LockOnTransform : MonoBehaviour
    {
        [ShowNonSerializedField] private PlayerCamera cam;

        private void Start()
        {
            cam = PlayerCamera.cam;
        }

        private void LateUpdate()
        {
            if (cam == null) return;
            transform.forward = Camera.main.transform.forward;
        }
    }
}
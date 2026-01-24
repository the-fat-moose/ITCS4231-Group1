using UnityEngine;

namespace Group1{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera cam;

        private void Awake()
        {
            if(cam == null)
            {
                cam = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

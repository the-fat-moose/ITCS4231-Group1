using UnityEngine;

namespace Group1 {
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;

namespace Group1 {
    public class Utility_DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] float timeUntilDestroyed = 5f;

        private void Awake()
        {
            Destroy(gameObject, timeUntilDestroyed);
        }
    }
}
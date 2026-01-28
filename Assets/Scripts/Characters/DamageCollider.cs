using UnityEngine;

namespace Group1 {
    public class DamageCollider : MonoBehaviour
    {
        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;

        private void OggerEnter(Collider other)
        {
            
        }
    }
}
using UnityEngine;

namespace Group1 
{
    public class AIFinalBossProjectileAttack : MonoBehaviour
    {
        public ParticleSystem particles;

        [HideInInspector] public Vector3 target;
        [HideInInspector] public float movementForce = 0f;
        [HideInInspector] public bool fireProjectile = false;

        private void Start()
        {
            Destroy(this, 5f);
        }

        private void Update()
        {
            if (fireProjectile)
            {
                float step = movementForce * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target, step);
            }
        }
    }
}
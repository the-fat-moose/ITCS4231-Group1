using UnityEngine;

namespace Group1 
{
    public class AIFinalBossProjectileAttack : MonoBehaviour
    {
        public ParticleSystem particles;

        [HideInInspector] public Vector3 direction;
        [HideInInspector] public float movementForce = 0f;
        [HideInInspector] public bool fireProjectile = false;

        private Rigidbody rb;

        private void Start()
        {
            Destroy(this, 5f);

            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (fireProjectile)
            {
                float step = movementForce * Time.deltaTime;

                Debug.Log("AIFinalBossProjectileAttack step: " + step);
                //transform.position = Vector3.MoveTowards(transform.position, target, step);

                if (rb != null)
                {
                    rb.linearVelocity = direction * step;
                }
            }
        }
    }
}
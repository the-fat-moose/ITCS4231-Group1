using UnityEngine;

namespace Group1 {
    public class FinalBossProjectileDamageCollider : DamageCollider
    {
        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null && damageTarget.characterGroup != CharacterGroup.Team02)
            {
                if (damageTarget.isInvulnerable) return;

                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // CHECK IF THE TARGET IS BLOCKING

                // CHECK IF THE TARGET IS INVULNERABLE
                CheckForBlock(damageTarget);

                DamageTarget(damageTarget);

                // DESTROY SELF ON IMPACT
                AIFinalBossProjectileAttack projectileAttack = GetComponent<AIFinalBossProjectileAttack>();
                if (projectileAttack != null)
                {
                    projectileAttack.particles.Stop();
                }

                Destroy(this, 1f);
            }
        }
    }
}
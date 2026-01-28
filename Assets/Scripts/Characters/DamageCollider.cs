using System.Collections.Generic;
using UnityEngine;

namespace Group1 {
    public class DamageCollider : MonoBehaviour
    {
        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;

        [Header("Contact Point")]
        protected Vector3 contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponent<CharacterManager>();

            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // CHECK IF THE TARGET IS BLOCKING

                // CHECK IF THE TARGET IS INVULNERABLE

                Debug.Log("COLLIDING WITH CHARACTER");

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            // WE DONT WANT TO DAMAGE THE SAME TARGET MORE THAN ONCE IN A SINGLE ATTACK
            // SO WE ADD THEM TO A LIST THAT CHECKS BEFORE APPLYING DAMAGE
            if (charactersDamaged.Contains(damageTarget)) return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }
    }
}
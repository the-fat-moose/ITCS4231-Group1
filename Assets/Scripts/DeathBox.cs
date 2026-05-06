using System.Collections.Generic;
using UnityEngine;

namespace Group1
{
    public class DeathBox : MonoBehaviour
    {
        protected Vector3 contactPoint;
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();
        
        void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                if (damageTarget.isInvulnerable) return;

                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            // WE DONT WANT TO DAMAGE THE SAME TARGET MORE THAN ONCE IN A SINGLE ATTACK
            // SO WE ADD THEM TO A LIST THAT CHECKS BEFORE APPLYING DAMAGE

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = 1000;
            damageEffect.magicDamage = 1000;
            damageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }
    }   
}
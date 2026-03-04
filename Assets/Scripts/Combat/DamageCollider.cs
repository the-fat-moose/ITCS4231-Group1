using System.Collections.Generic;
using UnityEngine;

namespace Group1 {
    public class DamageCollider : MonoBehaviour
    {
        [Header("Collider")]
        [SerializeField] protected Collider damageCollider;


        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;

        [Header("Contact Point")]
        protected Vector3 contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        [Header("Block")]
        protected Vector3 directionFromAttackToDamageTarget;
        protected float dotValueFromAttackToDamageTarget;

        protected virtual void Awake()
        {
            
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // CHECK IF THE TARGET IS BLOCKING

                // CHECK IF THE TARGET IS INVULNERABLE
                CheckForBlock(damageTarget);

                DamageTarget(damageTarget);
            }
        }

        protected virtual void CheckForBlock(CharacterManager damageTarget)
        {
            if(charactersDamaged.Contains(damageTarget)) return;

            GetBlockingDotValue(damageTarget);

            if (damageTarget.IsBlocking && dotValueFromAttackToDamageTarget > 0.3f)
            {
                charactersDamaged.Add(damageTarget);

                TakeBlockedDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeBlockedDamageEffect);

                damageEffect.physicalDamage = physicalDamage;
                damageEffect.magicDamage = magicDamage;

                damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
            }
        }

        protected virtual void GetBlockingDotValue(CharacterManager damageTarget)
        {
            directionFromAttackToDamageTarget = transform.position - damageTarget.transform.position;
            dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
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
            damageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        protected virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        protected virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            charactersDamaged.Clear(); // WE RESET THE CHARACTERS THAT HAVE BEEN HIT WHEN WE RESET THE COLLIDER SO THEY CAN BE HIT AGAIN
        }
    }
}
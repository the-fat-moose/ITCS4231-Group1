using UnityEngine;

namespace Group1 
{
    public class FinalBossMeleeWeaponDamageCollider : DamageCollider
    {
        [SerializeField] AIBossCharacterManager AICharacter;
        protected override void Awake()
        {
            base.Awake();

            damageCollider = GetComponent<Collider>();
            AICharacter = GetComponentInParent<AIBossCharacterManager>();
        }

        protected override void GetBlockingDotValue(CharacterManager damageTarget)
        {
            directionFromAttackToDamageTarget = AICharacter.transform.position - damageTarget.transform.position;
            dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget)) return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        public void EnableCollider()
        {
            EnableDamageCollider();
        }

        public void DisableCollider()
        {
            DisableDamageCollider();
        }
    }
}
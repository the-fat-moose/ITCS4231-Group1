using UnityEngine;

namespace Group1 {
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage; // (when calculating damage, this is used to check for attacker damage modifiers, effects, etc.)

        [Header("Weapon Attack Modifiers")]
        public float light_Attack_Modifier;
        public float heavy_Attack_Modifier;

        protected override void Awake()
        {
            base.Awake();

            damageCollider.enabled = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                if(damageTarget == characterCausingDamage) return;

                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // CHECK IF THE TARGET IS BLOCKING

                // CHECK IF THE TARGET IS INVULNERABLE

                DamageTarget(damageTarget);
            }
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget)) return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;

            switch (characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.Light01:
                    ApplyAttackDamageModifiers(light_Attack_Modifier, damageEffect);
                    break;
                case AttackType.Light02:
                    ApplyAttackDamageModifiers(light_Attack_Modifier, damageEffect);
                    break;
                case AttackType.Light03:
                    ApplyAttackDamageModifiers(light_Attack_Modifier, damageEffect);
                    break;
                case AttackType.Heavy01:
                    ApplyAttackDamageModifiers(heavy_Attack_Modifier, damageEffect);
                    break;
                case AttackType.Heavy02:
                    ApplyAttackDamageModifiers(heavy_Attack_Modifier, damageEffect);
                    break;
                default:
                    break;
            }

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
        {
            damage.physicalDamage *= modifier;
            damage.magicDamage *= modifier;
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
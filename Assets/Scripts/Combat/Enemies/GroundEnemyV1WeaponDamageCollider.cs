using UnityEngine;

namespace Group1 {
    public class GroundEnemyV1WeaponDamageCollider : DamageCollider
    {
        protected override void Awake()
        {
            base.Awake();

            damageCollider = GetComponent<Collider>();
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget)) return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;

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
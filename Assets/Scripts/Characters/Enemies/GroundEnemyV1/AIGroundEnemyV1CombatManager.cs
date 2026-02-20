using UnityEngine;

namespace Group1 {
    public class AIGroundEnemyV1CombatManager : AiCharacterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] MeleeWeaponDamageCollider meleeWeaponDamageCollider;

        [Header("Damage")]
        [SerializeField] int baseDamage = 25;
        [SerializeField] float attack01DamageModifier = 1.0f;
        [SerializeField] float attack02DamageModifier = 1.4f;

        public void SetAttack01Damage()
        {
            meleeWeaponDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        }

        public void SetAttack02Damage()
        {
            meleeWeaponDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        }

        public void OpenMeleeWeaponDamageCollider()
        {
            meleeWeaponDamageCollider.EnableCollider();
        }

        public void CloseMeleeWeaponDamageCollider()
        {
            meleeWeaponDamageCollider.DisableCollider();
        }
    }
}
using UnityEngine;

namespace Group1 {
    public class AIGroundEnemyV1CombatManager : AiCharacterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] GroundEnemyV1WeaponDamageCollider groundEnemyV1WeaponDamageCollider;

        [Header("Damage")]
        [SerializeField] int baseDamage = 25;
        [SerializeField] float attack01DamageModifier = 1.0f;
        [SerializeField] float attack02DamageModifier = 1.4f;

        public void SetAttack01Damage()
        {
            groundEnemyV1WeaponDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        }

        public void SetAttack02Damage()
        {
            groundEnemyV1WeaponDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        }

        public void OpenMeleeWeaponDamageCollider()
        {
            groundEnemyV1WeaponDamageCollider.EnableCollider();
        }

        public void CloseMeleeWeaponDamageCollider()
        {
            groundEnemyV1WeaponDamageCollider.DisableCollider();
        }
    }
}
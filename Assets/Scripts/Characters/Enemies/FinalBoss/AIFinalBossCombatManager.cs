using UnityEngine;

namespace Group1 {
    public class AIFinalBossCombatManager : AiCharacterCombatManager
    {
        [Header("Damage")]
        [SerializeField] FinalBossMeleeWeaponDamageCollider finalBossMeleeWeaponDamageCollider;

        [Header("Damage")]
        [SerializeField] int baseDamage = 25;
        [SerializeField] float attack01DamageModifier = 1.0f;
        [SerializeField] float attack02DamageModifier = 1.4f;

        public void SetAttack01Damage()
        {
            finalBossMeleeWeaponDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        }

        public void SetAttack02Damage()
        {
            finalBossMeleeWeaponDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        }

        public void OpenMeleeWeaponDamageCollider()
        {
            finalBossMeleeWeaponDamageCollider.EnableCollider();
        }

        public void CloseMeleeWeaponDamageCollider()
        {
            finalBossMeleeWeaponDamageCollider.DisableCollider();
        }
    }
}
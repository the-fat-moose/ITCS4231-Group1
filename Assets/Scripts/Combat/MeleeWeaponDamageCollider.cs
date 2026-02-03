using UnityEngine;

namespace Group1 {
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage; // (when calculating damage, this is used to check for attacker damage modifiers, effects, etc.)
    }
}
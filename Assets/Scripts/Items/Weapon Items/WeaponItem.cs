using UnityEngine;

namespace Group1 {
    public class WeaponItem : Item
    {
        // ANIMATOR CONTROLLER OVERRIDE (Change attack animations based on weapon you are currently using)

        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirements")]
        public int strengthREQ = 0;
        public int dexREQ = 0;
        public int intREQ = 0;

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;
        public int magicDamage = 0;

        // WEAPON GUARD ABSORPTIONS
        [Header("Attack Modifier")]
        public float light_Attack_01_Modifier = 1.1f;
        // LIGHT ATTACK MODIFIER
        // HEAVY ATTACK MODIFIER
        // CRITICAL DAMAGE MODIFIER ETC

        [Header("Stamina Costs")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaModifier = 0.9f;
        // RUNNING ATTACK STAMINA COST MODIFIER
        // LIGHT ATTACK STAMINA COST MODIFIER
        // HEAVY ATTACK STAMINA COST MODIFIER

        [Header("Actions")]
        public WeaponItemAction rb_Action;

        // ABILITIES

        // BLOCKING SOUNDS
    }
}
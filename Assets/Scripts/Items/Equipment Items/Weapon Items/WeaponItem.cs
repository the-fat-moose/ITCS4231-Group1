using UnityEngine;

namespace Group1 {
    public class WeaponItem : EquipmentItem
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

        [Header("Blocking Absorption")]
        public int physicalDamageAbsorption = 70;
        public int magicDamageAbsorption = 70;

        // WEAPON GUARD ABSORPTIONS
        [Header("Attack Modifier")]
        public float light_Attack_Modifier = 1.1f;
        public float heavy_Attack_Modifier = 2f;
        // CRITICAL DAMAGE MODIFIER ETC

        [Header("Stamina Costs")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaModifier = 0.9f;
        public float heavyAttackStaminaModifier = 2f;
        // RUNNING ATTACK STAMINA COST MODIFIER

        [Header("Actions")]
        public WeaponItemAction rb_Action;
        public WeaponItemAction rt_Action;
        public WeaponItemAction lb_Action;

        // ABILITIES

        // BLOCKING SOUNDS
    }
}
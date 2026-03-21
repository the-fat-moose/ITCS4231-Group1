using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Items/Equipment/Lumen")]
    public class LumenEquipmentItem : EquipmentItem
    {
        [Header("Modifiers")]
        public float vitalityMultiplier = 1f;
        public float mindMultipler = 1f;
        public float staminaRegenMultiplier = 1f;
        
        public float physicalDamageMultiplier = 1f;
        public float magicDamageMultiplier = 1f;
        
        public float parryWindowMultiplier = 1f;
        public float flaskHealingMultiplier = 1f;
        
        public float manaRestoreOnHitMultiplier = 1f;
        public float manaRestoneOnParryMultiplier = 1f;

        public bool useLowHealthDamageBoost = false;
        public float lowHealthThreshhold = 0f;
        public float lowHealthDamageMultiplier = 1f;
    }
}
using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Items/Lumens/Lumen Item")]
    public class LumenItem : EquipmentItem
    {
        // MODIFIES THE PLAYERS ON PERSON STATS
        [Header("Stat Modifiers")]
        public float hpModifier = 0f;
        public float manaModifier = 0f;
        public float staminaRegenerationRateModifier = 0f;
        public float flaskRestorationAmountModifier = 0f;

        // MODIFIES THE DAMAGE OR ABILITY FEATURES OF THE PLAYER
        [Header("Combat Modifiers")]
        public float physicalDamageModifier = 0f;
        public float magicDamageModifier = 0f;
        public float manaRegenerationAmountModifier = 0f;

        // CONDITIONAL MODIFIERS THAT AFFECT HOW THE ABOVE MODIFIERS ARE APPLIED, 
        // PLUS OTHER MODIFIERS BASED ON BOOLEAN LOGIC INSTEAD OF MATH LOGIC
        [Header("Conditional Modifiers")]
        public bool increaseDamageAtLowHP = false;
        public bool increaseParryWindow = false;

        // LUMEN MODEL

        // ADD LOGIC FOR ON EQUIP
    }
}
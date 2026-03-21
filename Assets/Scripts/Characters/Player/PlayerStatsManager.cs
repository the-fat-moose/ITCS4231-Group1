using UnityEngine;

namespace Group1 {
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            CalculateHealthBasedOnVitalityLevel(player.Vitality);
            CalculateStaminaBasedOnEnduranceLevel(player.Endurance);
            CalculateManaBasedOnMindLevel(player.Mind);
        }

        public void CalculateNewStats()
        {
            float vitalityMultiplier = 1f;
            float mindMultipler = 1f;
            float staminaRegenMultiplier = 1f;
            
            /*
            float physicalDamageMultiplier = 1f;
            float magicDamageMultiplier = 1f;
            
            float parryWindowMultiplier = 1f;
            float flaskHealingMultiplier = 1f;
            
            float manaRestoreOnHitMultiplier = 1f;
            float manaRestoneOnParryMultiplier = 1f;

            bool useLowHealthDamageBoost = false;
            float lowHealthThreshhold = 0f;
            float lowHealthDamageMultiplier = 1f;
            */

            if (player.playerInventoryManager.lumenEquipmentItemSlots.Length > 0)
            {
                for (int i = 0; i < player.playerInventoryManager.lumenEquipmentItemSlots.Length; i++)
                {
                    if (player.playerInventoryManager.lumenEquipmentItemSlots[i] != null)
                    {
                        vitalityMultiplier *= player.playerInventoryManager.lumenEquipmentItemSlots[i].vitalityMultiplier;
                        mindMultipler *= player.playerInventoryManager.lumenEquipmentItemSlots[i].mindMultipler;
                        staminaRegenMultiplier *= player.playerInventoryManager.lumenEquipmentItemSlots[i].staminaRegenMultiplier;
                    }
                }
            }

            CalculateHealthBasedOnVitalityLevel((int)Mathf.Ceil(player.Vitality * vitalityMultiplier));
            CalculateManaBasedOnMindLevel((int)Mathf.Ceil(player.Mind * mindMultipler));
            staminaRegenerationAmount = Mathf.RoundToInt(staminaRegenerationAmount * staminaRegenMultiplier);
        }
    }
}
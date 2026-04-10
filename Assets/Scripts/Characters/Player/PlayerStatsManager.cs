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

        public void CalculateLumenEquippedStatModifiers()
        {
            // RESET ALL VALUES
            maxHealthMultiplier = baseHealthMultiplier; // 1
            maxManaMultiplier = baseManaMultiplier; // 1

            staminaRegenMultiplier = baseStaminaRegenMultiplier; // 1
            manaRegenMultiplier = baseManaRegenMultiplier; // 1

            player.playerCombatManager.magicDamageMultiplier = player.playerCombatManager.baseMagicDamageMultiplier;

            player.flaskRestorationMultiplier = player.baseFlaskRestorationMultiplier;

            // LUMEN SLOT 1
            if (player.playerInventoryManager.lumenSlot1Item != null)
            {
                maxHealthMultiplier += player.playerInventoryManager.lumenSlot1Item.hpModifier;
                maxManaMultiplier += player.playerInventoryManager.lumenSlot1Item.manaModifier;

                staminaRegenMultiplier += player.playerInventoryManager.lumenSlot1Item.staminaRegenerationRateModifier;
                manaRegenMultiplier += player.playerInventoryManager.lumenSlot1Item.manaRegenerationAmountModifier;

                player.playerCombatManager.magicDamageMultiplier += player.playerInventoryManager.lumenSlot1Item.magicDamageModifier;

                player.flaskRestorationMultiplier += player.playerInventoryManager.lumenSlot1Item.flaskRestorationAmountModifier;
            }
            // LUMEN SLOT 2
            if (player.playerInventoryManager.lumenSlot2Item != null)
            {
                maxHealthMultiplier += player.playerInventoryManager.lumenSlot2Item.hpModifier;
                maxManaMultiplier += player.playerInventoryManager.lumenSlot2Item.manaModifier;

                staminaRegenMultiplier += player.playerInventoryManager.lumenSlot2Item.staminaRegenerationRateModifier;
                manaRegenMultiplier += player.playerInventoryManager.lumenSlot2Item.manaRegenerationAmountModifier;

                player.playerCombatManager.magicDamageMultiplier += player.playerInventoryManager.lumenSlot2Item.magicDamageModifier;

                player.flaskRestorationMultiplier += player.playerInventoryManager.lumenSlot2Item.flaskRestorationAmountModifier;
            }
            // LUMEN SLOT 3
            if (player.playerInventoryManager.lumenSlot3Item != null)
            {
                maxHealthMultiplier += player.playerInventoryManager.lumenSlot3Item.hpModifier;
                maxManaMultiplier += player.playerInventoryManager.lumenSlot3Item.manaModifier;

                staminaRegenMultiplier += player.playerInventoryManager.lumenSlot3Item.staminaRegenerationRateModifier;
                manaRegenMultiplier += player.playerInventoryManager.lumenSlot3Item.manaRegenerationAmountModifier;

                player.playerCombatManager.magicDamageMultiplier += player.playerInventoryManager.lumenSlot3Item.magicDamageModifier;

                player.flaskRestorationMultiplier += player.playerInventoryManager.lumenSlot3Item.flaskRestorationAmountModifier;
            }
            // LUMEN SLOT 4
            if (player.playerInventoryManager.lumenSlot4Item != null)
            {
                maxHealthMultiplier += player.playerInventoryManager.lumenSlot4Item.hpModifier;
                maxManaMultiplier += player.playerInventoryManager.lumenSlot4Item.manaModifier;

                staminaRegenMultiplier += player.playerInventoryManager.lumenSlot4Item.staminaRegenerationRateModifier;
                manaRegenMultiplier += player.playerInventoryManager.lumenSlot4Item.manaRegenerationAmountModifier;

                player.playerCombatManager.magicDamageMultiplier += player.playerInventoryManager.lumenSlot4Item.magicDamageModifier;

                player.flaskRestorationMultiplier += player.playerInventoryManager.lumenSlot4Item.flaskRestorationAmountModifier;
            }
        
            player.RecalibrateStatValues();
        }
    }
}
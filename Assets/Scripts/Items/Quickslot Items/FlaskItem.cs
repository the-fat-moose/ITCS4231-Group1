using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Items/Consumables/Life Shard")]
    public class FlaskItem : QuickSlotItem
    {
        [Header("Flask Type")]
        public bool healthFlask = true;

        [Header("Restoration Value")]
        private int flaskRestoration = 50;

        [Header("Empty Item")]
        public GameObject emptyFlaskModel;
        public string emptyFlaskAnimation;

        public override bool CanIUseThisItem(PlayerManager player)
        {
            if (!player.playerCombatManager.isUsingItem && player.isPerformingAction) return false;

            if (player.isAttacking) return false;

            return true;
        }

        public override void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player)) return;

            // HEALTH FLASK COUNT CHECK
            if (healthFlask && player.remainingHealthFlasks <= 0)
            {
                if (player.playerCombatManager.isUsingItem) return;

                player.playerCombatManager.isUsingItem = true;

                player.HideWeapons();

                Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                GameObject emptyFlask = Instantiate(emptyFlaskModel, player.playerEquipmentManager.rightHandSlot.transform);
                player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;

                player.playerAnimatorManager.PlayTargetActionAnimation(emptyFlaskAnimation, false, false, true, true, false);
                return;
            }

            // CHECK FOR CHUGGING
            if (player.playerCombatManager.isUsingItem)
            {
                player.IsChugging = true;

                return;
            }

            player.playerCombatManager.isUsingItem = true;

            player.playerEffectsManager.activeQuickSlotItemFX = Instantiate(itemModel, player.playerEquipmentManager.rightHandSlot.transform);

            player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, false, false, true, true, false);

            player.HideWeapons();
        }

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            if (healthFlask)
            {
                player.CurrentHealth += Mathf.FloorToInt(flaskRestoration * player.flaskRestorationMultiplier);
                player.remainingHealthFlasks -= 1;
            }

            if (healthFlask && player.remainingHealthFlasks <= 0)
            {
                Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
                GameObject emptyFlask = Instantiate(emptyFlaskModel, player.playerEquipmentManager.rightHandSlot.transform);
                player.playerEffectsManager.activeQuickSlotItemFX = emptyFlask;
            }

            PlayHealingFX(player);
        }

        private void PlayHealingFX(PlayerManager player)
        {
            Instantiate(WorldCharacterEffectsManager.instance.healingFlaskVFX, player.transform);
            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.healingSFX);
        }

        public override int GetCurrentAmount(PlayerManager player)
        {
            int currentAmount = 0;

            if (healthFlask)
                currentAmount = player.remainingHealthFlasks;

            return currentAmount;
        }
    }
}
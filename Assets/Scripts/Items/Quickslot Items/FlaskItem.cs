using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Items/Consumables/Life Shard")]
    public class FlaskItem : QuickSlotItem
    {
        [Header("Flask Type")]
        [SerializeField] bool healthFlask = true;

        [Header("Restoration Value")]
        private int flaskRestoration = 50;
        public float flaskRestorationMultiplier = 1f;

        [Header("Empty Item")]
        [SerializeField] GameObject emptyFlaskModel;

        public override bool CanIUseThisItem(PlayerManager player)
        {
            if (healthFlask && player.remainingHealthFlasks <= 0) return false;

            if (!healthFlask && player.remainingHealthFlasks <= 0) return false;

            return true;
        }

        public override void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player)) return;

            player.playerEffectsManager.activeQuickSlotItemFX = Instantiate(itemModel, player.playerEquipmentManager.rightHandSlot.transform);

            player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, true, false, true, false, false);

            player.HideWeapons();
        }

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            if (healthFlask)
            {
                player.CurrentHealth += Mathf.FloorToInt(flaskRestoration * flaskRestorationMultiplier);
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
    }
}
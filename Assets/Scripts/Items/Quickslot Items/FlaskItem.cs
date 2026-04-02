using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Items/Consumables/Life Shard")]
    public class FlaskItem : QuickSlotItem
    {
        [Header("Empty Item")]
        [SerializeField] GameObject emptyFlaskModel;

        public override void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player)) return;

            player.playerEffectsManager.activeQuickSlotItemFX = Instantiate(itemModel, player.playerEquipmentManager.rightHandSlot.transform);

            player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, true, false, true, true, false);
        }
    }
}
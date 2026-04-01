using UnityEngine;

namespace Group1
{
    public class QuickSlotItem : Item
    {
        [Header("Item Model")]
        [SerializeField] GameObject itemModel;

        [Header("Animation")]
        [SerializeField] string useItemAnimation;

        public virtual void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player)) return;

            player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, true);
        }

        public virtual bool CanIUseThisItem(PlayerManager player)
        {
            return true;
        }
    }
}
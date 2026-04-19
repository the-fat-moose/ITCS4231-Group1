using UnityEngine;

namespace Group1
{
    public class QuickSlotItem : Item
    {
        [Header("Item Model")]
        [SerializeField] protected GameObject itemModel;

        [Header("Animation")]
        [SerializeField] protected string useItemAnimation;

        // NOT ALL QUICK SLOT ITEMS ARE CONSUMABLE
        [Header("Consumable")]
        public bool isConsumable = true;
        public int itemAmount = 1;

        public virtual void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player)) return;

            player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, true, false, true, true);
        }

        public virtual void SuccessfullyUseItem(PlayerManager player)
        {
            
        }

        public virtual bool CanIUseThisItem(PlayerManager player)
        {
            return true;
        }

        public virtual int GetCurrentAmount(PlayerManager player)
        {
            return 0;
        }
    }
}
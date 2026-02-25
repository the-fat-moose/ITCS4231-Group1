using Group1;
using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Block Action")]
    public class BlockAction : WeaponItemAction
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if(!playerPerformingAction.playerCombatManager.canBlock) return;

            if (playerPerformingAction.isAttacking)
            {
                playerPerformingAction.isBlocking = false;

                return;
            }

            if(playerPerformingAction.isBlocking) return;

            playerPerformingAction.isBlocking = true;
        }
    }
}

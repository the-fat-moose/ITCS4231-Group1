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
                playerPerformingAction.IsBlocking = false;

                return;
            }

            if(playerPerformingAction.IsBlocking) return;

            playerPerformingAction.IsBlocking = true;
            playerPerformingAction.animator.CrossFade("PlayerCharacter_InitialBlock", 0.2f);

        }
    }
}

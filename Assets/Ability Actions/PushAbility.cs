using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Character Actions/Player Abilities/Kick Ability")]
    public class PushAbility : PlayerAbilityAction
    {
        public override void AttemptToPerformAbility(PlayerManager player)
        {
            base.AttemptToPerformAbility(player);

            if (!player.isGrounded) return;
            if (player.isPerformingAction) return;

            //player.playerAnimatorManager.PlayTargetActionAnimation(kickAnimation, true);
        }
    }   
}
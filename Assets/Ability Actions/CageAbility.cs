using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Character Actions/Player Abilities/Cage Ability")]
    public class CageAbility : PlayerAbilityAction
    {
        [Header("VFX")]
        public GameObject cageVFX;

        [Header("Animation")]
        public string cageAnimation = "PlayerCharacter_Ability_Freeze";

        public override void AttemptToPerformAbility(PlayerManager player)
        {
            base.AttemptToPerformAbility(player);

            if (player.isPerformingAction) return;
            if (!player.isGrounded) return;

            player.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.CageAbility, cageAnimation, true);
        }

    }
}

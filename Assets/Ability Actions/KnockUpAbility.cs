using Group1;
using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Character Actions/Player Abilities/Knock Up Ability")]
    public class KnockUpAbility : PlayerAbilityAction
    {
        [Header("VFX")]
        public GameObject knockUpVFX;
        public Vector3 vfxOffset = new Vector3(0, 1f, 1f);

        [Header("Animation")]
        public string knockUpAnimation = "PlayerCharacter_Ability_Freeze";

        public override void AttemptToPerformAbility(PlayerManager player)
            {
                base.AttemptToPerformAbility(player);

                if (player.isPerformingAction) return;
                if (!player.isGrounded) return;

                player.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.PushAbility, knockUpAnimation, true);
            }
    }
}

using UnityEngine;

namespace Group1 {
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string light_Attack_01 = "PlayerCharacter_LightAttack01";
        [SerializeField] string light_Attack_02 = "PlayerCharacter_LightAttack02";
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (playerPerformingAction.playerCombatManager.isUsingItem) return;

            // CHECK FOR STOPS (ex. no stamina)
            if (playerPerformingAction.CurrentStamina <= 0) return;

            if (!playerPerformingAction.isGrounded) return; // CHANGE THIS WHEN AERIAL ATTACKS ARE ADDED

            playerPerformingAction.isAttacking = true;

            PerformLightAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            // If attacking but combo window is closed → wait for combo window
            if (playerPerformingAction.isPerformingAction &&
                !playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon)
            {
                // Do NOT restart the animation
                return;
            }

            // Combo window open → perform combo
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon)
            {
                // Prevent multiple combo triggers
                if (playerPerformingAction.playerCombatManager.hasConsumedComboInput)
                    return;

                playerPerformingAction.playerCombatManager.hasConsumedComboInput = true;

                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.Light02, light_Attack_02, true);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.Light01, light_Attack_01, true);
                }

                PlayerInputManager.inputs.ClearQueuedInputs();
                return;
            }


            // Not attacking → start first attack
            if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.Light01, light_Attack_01, true);
                return;
            }
        }


    }
}
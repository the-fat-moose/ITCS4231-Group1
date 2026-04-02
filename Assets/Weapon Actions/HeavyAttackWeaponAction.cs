using UnityEngine;

namespace Group1{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttackWeaponAction : WeaponItemAction
    {
        [SerializeField] string heavy_Attack_01 = "PlayerCharacter_HeavyAttack_Hold";
        [SerializeField] string heavy_Attack_02 = "PlayerCharacter_HeavyAttack_02";
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (playerPerformingAction.playerCombatManager.isUsingItem) return;
            
            // CHECK FOR STOPS (ex. no stamina)
            if (playerPerformingAction.CurrentStamina <= 0) return;

            if (!playerPerformingAction.isGrounded) return; // CHANGE THIS WHEN AERIAL ATTACKS ARE ADDED

            playerPerformingAction.isAttacking = true;

            PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            //If we are attacking already do combo
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                if(playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.Heavy02, heavy_Attack_02, true);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.Heavy01, heavy_Attack_01, true);
                }
            }
            else if(!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.Heavy01, heavy_Attack_01, true);

                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = true;
            }
        }
    }
}


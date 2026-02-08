using UnityEngine;

namespace Group1{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttackWeaponAction : WeaponItemAction
    {
        [SerializeField] string heavy_Attack_01 = "PlayerCharacter_HeavyAttack_Hold";
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);
            
            // CHECK FOR STOPS (ex. no stamina)
            if (playerPerformingAction.CurrentStamina <= 0) return;

            if (!playerPerformingAction.isGrounded) return; // CHANGE THIS WHEN AERIAL ATTACKS ARE ADDED

            PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.Heavy, heavy_Attack_01, true);
        }
    }
}


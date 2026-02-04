using UnityEngine;

namespace Group1 {
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] string light_Attack_01 = "PlayerCharacter_LightAttack";
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            // CHECK FOR STOPS (ex. no stamina)
            if (playerPerformingAction.CurrentStamina <= 0) return;

            if (!playerPerformingAction.isGrounded) return; // CHANGE THIS WHEN AERIAL ATTACKS ARE ADDED

            PerformLightAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.Light, light_Attack_01, true);
        }
    }
}
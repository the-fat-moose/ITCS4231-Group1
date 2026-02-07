using UnityEngine;

namespace Group1 {
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            // PERFORM THE ACTION
            //weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

            player.PerformWeaponBasedAction(weaponAction.actionID, weaponPerformingAction.itemID);
        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            float staminaLoss = 0;

            if(currentWeaponBeingUsed == null) return;

            switch (currentAttackType)
            {
                case AttackType.Light:
                    staminaLoss = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaModifier;
                    break;
                default:
                    break;
            }

            player.CurrentStamina -= Mathf.RoundToInt(staminaLoss);
        }

        public virtual void SetTarget(CharacterManager newTarget){
            base.SetTarget(newTarget);

            PlayerCamera.cam.SetLockCameraHeight();
        }
    }
}
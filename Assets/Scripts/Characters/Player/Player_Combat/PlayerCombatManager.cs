using Unity.VisualScripting;
using UnityEngine;

namespace Group1 {
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;

        [Header("Flags")]
        public bool canComboWithMainHandWeapon = true;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            // PERFORM THE ACTION
            //weaponAction.AttemptToPerformAction(player, weaponPerformingAction);
            if (weaponAction != null && weaponPerformingAction != null) {
                player.PerformWeaponBasedAction(weaponAction.actionID, weaponPerformingAction.itemID);
            }
        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            Debug.Log("DrainStaminaBasedOnAttack CALLED");

            float staminaLoss = 0;

            if(currentWeaponBeingUsed == null) return;

            switch (currentAttackType)
            {
                case AttackType.Light01:
                    staminaLoss = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaModifier;
                    break;
                case AttackType.Light02:
                    staminaLoss = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaModifier;
                    break;
                case AttackType.Light03:
                    staminaLoss = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaModifier;
                    break;
                case AttackType.Heavy01:
                    staminaLoss = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaModifier;
                    break;
                default:
                    break;
            }

            player.CurrentStamina -= Mathf.RoundToInt(staminaLoss);
        }

        public override void SetTarget(CharacterManager newTarget){
            base.SetTarget(newTarget);

            PlayerCamera.cam.SetLockCameraHeight();
        }
    
    }
}
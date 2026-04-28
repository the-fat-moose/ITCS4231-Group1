using Unity.VisualScripting;
using UnityEngine;

namespace Group1 {
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;

        [Header("Flags")]
        public bool canComboWithMainHandWeapon = true;
        public bool isUsingItem = false;
        public bool hasConsumedComboInput = false;


        [Header("ability")]
        public float pushAbilityManaCost = 20;
        public float knockUpAbilityManaCost = 30;
        public float cageAbilityManaCost = 40;
        public float aoeAbilityManaCost = 30;
        public float abilityDamage = 20;
 
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

        public void PerformAbilityAction(PlayerAbilityAction abilityAction)
        {
            // PERFORM THE ACTION
            //abilityAction.AttemptToPerformAbility(player);
            if (abilityAction != null) {
                player.PerformAbilityAction(abilityAction.actionID);
            }
        }

        public virtual void DrainStaminaBasedOnAttack()
        {
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
                case AttackType.Heavy02:
                    staminaLoss = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaModifier;
                    break;
                default:
                    break;
            }

            player.CurrentStamina -= Mathf.RoundToInt(staminaLoss);
        }

        public virtual void DrainManaBasedOnAbility()
        {
            float manaLoss = 0;

            switch (currentAttackType)
            {
                case AttackType.PushAbility:
                    manaLoss = pushAbilityManaCost;
                    break;
                case AttackType.KnockUpAbility:
                    manaLoss = knockUpAbilityManaCost;
                    break;
                case AttackType.CageAbility:
                    manaLoss = cageAbilityManaCost;
                    break;
                case AttackType.AOEAbility:
                    manaLoss = aoeAbilityManaCost;
                    break;
                default:
                    break;
            }

            player.CurrentMana -= Mathf.RoundToInt(manaLoss);
        }

        public override void SetTarget(CharacterManager newTarget){
            base.SetTarget(newTarget);

            PlayerCamera.cam.SetLockCameraHeight();
        }

        // QUICK SLOT
        public void SuccessfullyUseQuickSlotItem()
        {
            if (player.playerInventoryManager.currentQuickSlotItem != null)
                player.playerInventoryManager.currentQuickSlotItem.SuccessfullyUseItem(player);
        }
    }
}
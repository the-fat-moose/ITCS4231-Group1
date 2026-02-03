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
    }
}
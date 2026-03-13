using System.Collections;
using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Parry Action")]
    public class ParryAction : WeaponItemAction
    {
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);
            if(!playerPerformingAction.playerCombatManager.canParry) return;
            playerPerformingAction.StartCoroutine(ParryTimer(playerPerformingAction));

        }

        private IEnumerator ParryTimer(PlayerManager player)
        {
            player.characterCombatManager.canParry = true;
            yield return new WaitForSeconds(player.characterCombatManager.parryWindow);
            player.characterCombatManager.canParry = false;
        }
    }
}


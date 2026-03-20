using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Character Actions/Player Abilities/Cage Ability")]
    public class CageAbility : PlayerAbilityAction
    {
        [Header("VFX")]
        public GameObject cageVFX;

        [Header("Animation")]
        public string cageAnimation = "PlayerCharacter_Ability_Freeze";


    }
}

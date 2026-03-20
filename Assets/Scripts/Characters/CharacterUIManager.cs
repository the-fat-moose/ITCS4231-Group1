using UnityEngine;

namespace Group1
{
    public class CharacterUIManager : MonoBehaviour
    {
        [Header("UI")]
        public bool hasFloatingHPBar = true;
        public UI_Character_HP_Bar characterHPBar;

        public void OnHPChanged(int oldValue, int newValue)
        {
            Debug.Log(this.gameObject + ": CharacterUIManager.OnHPChanged oldValue: " + oldValue + ", newValue: " + newValue);

            characterHPBar.oldHealthValue = oldValue;
            characterHPBar.SetStat(newValue);
        }
    }
}
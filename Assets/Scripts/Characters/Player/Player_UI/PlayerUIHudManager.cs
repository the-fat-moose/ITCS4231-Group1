using UnityEngine;
using UnityEngine.UI;

namespace Group1 {
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup[] canvasGroup;

        [Header("Stat Bars")]
        [SerializeField] UI_StatBar healthBar;
        [SerializeField] UI_StatBar manaBar;
        [SerializeField] UI_StatBar staminaBar;

        [Header("Quick Slots")]
        [SerializeField] Image weaponQuickSlotIcon;
        [SerializeField] Image quickSlotItemQuickSlotIcon;

        [Header("Boss Health Bar")]
        public Transform bossHealthBarParent;
        public GameObject bossHealthBarObject;

        public void ToggleHUD(bool status)
        {
            if (status)
            {
                foreach (var canvas in canvasGroup)
                {
                    canvas.alpha = 1;
                }
            }
            else
            {
                foreach (var canvas in canvasGroup)
                {
                    canvas.alpha = 0;
                }
            }
        }

        public void RefreshHUD()
        {
            
        }

        public void SetNewHealthValue(int oldValue, int newValue)
        {
            healthBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxHealthValue(int maxHealth)
        {
            healthBar.SetMaxStat(Mathf.RoundToInt(maxHealth));
        }

        public void SetNewManaValue(int oldValue, int newValue)
        {
            manaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxManaValue(int maxMana)
        {
            manaBar.SetMaxStat(maxMana);
        }

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(int maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }
    
        public void SetWeaponQuickSlotIcon(int weaponID)
        {
            WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);

            if (WorldItemDatabase.instance.GetWeaponByID(weaponID) == null)
            {
                Debug.Log("ITEM IS NULL");
                weaponQuickSlotIcon.enabled = false;
                weaponQuickSlotIcon.sprite = null;
                return;
            }

            if (weapon.itemIcon == null)
            {
                Debug.Log("ITEM HAS NO ICON");
                weaponQuickSlotIcon.enabled = false;
                weaponQuickSlotIcon.sprite = null;
                return;
            }

            weaponQuickSlotIcon.sprite = weapon.itemIcon;
            weaponQuickSlotIcon.enabled = true;
        }

        public void SetQuickSlotItemQuickSlotIcon(int quickSlotItemID)
        {
            QuickSlotItem quickSlotItem = WorldItemDatabase.instance.GetQuickSlotItemByID(quickSlotItemID);

            if (WorldItemDatabase.instance.GetQuickSlotItemByID(quickSlotItemID) == null)
            {
                Debug.Log("ITEM IS NULL");
                quickSlotItemQuickSlotIcon.enabled = false;
                quickSlotItemQuickSlotIcon.sprite = null;
                return;
            }

            if (quickSlotItem.itemIcon == null)
            {
                Debug.Log("ITEM HAS NO ICON");
                quickSlotItemQuickSlotIcon.enabled = false;
                quickSlotItemQuickSlotIcon.sprite = null;
                return;
            }

            // TO DO, UPDATE QUANTITY LEFT, SHOW IN UI
            // FADE OUT ICON IF NONE REMAIN

            quickSlotItemQuickSlotIcon.sprite = quickSlotItem.itemIcon;
            quickSlotItemQuickSlotIcon.enabled = true;
        }
    }
}

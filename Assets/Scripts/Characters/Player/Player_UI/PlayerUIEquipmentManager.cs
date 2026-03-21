using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Group1
{
    public class PlayerUIEquipmentManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] private GameObject menu;

        [Header("Weapon Slots")]
        [SerializeField] private Image rightHandSlot01;
        [SerializeField] private Image rightHandSlot02;
        [SerializeField] private Image rightHandSlot03;

        public void OpenEquipmentMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = true;
            menu.SetActive(true);

            RefreshWeaponSlotIcons();
        }

        public void CloseEquipmentMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

        private void RefreshWeaponSlotIcons()
        {
            PlayerManager player = FindFirstObjectByType<PlayerManager>();

            if (player != null)
            {
                // RIGHT WEAPON 01
                WeaponItem rightHandWeapon01 = player.playerInventoryManager.weaponsInRightHandSlots[0];

                if (rightHandWeapon01.itemIcon != null)
                {
                    rightHandSlot01.enabled = true;
                    rightHandSlot01.sprite = rightHandWeapon01.itemIcon;
                }
                else
                {
                    rightHandSlot01.enabled = false;
                }

                // RIGHT WEAPON 02
                WeaponItem rightHandWeapon02 = player.playerInventoryManager.weaponsInRightHandSlots[1];

                if (rightHandWeapon02.itemIcon != null)
                {
                    rightHandSlot02.enabled = true;
                    rightHandSlot02.sprite = rightHandWeapon02.itemIcon;
                }
                else
                {
                    rightHandSlot02.enabled = false;
                }

                // RIGHT WEAPON 03
                WeaponItem rightHandWeapon03 = player.playerInventoryManager.weaponsInRightHandSlots[2];

                if (rightHandWeapon03.itemIcon != null)
                {
                    rightHandSlot03.enabled = true;
                    rightHandSlot03.sprite = rightHandWeapon03.itemIcon;
                }
                else
                {
                    rightHandSlot03.enabled = false;
                }
            }
        }
    }
}
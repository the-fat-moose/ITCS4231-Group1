using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        // THIS INVENTORY POPULATES WITH RELATED ITEMS WHEN CHANGING EQUIPMENT
        [Header("Equipment Inventory")]
        [SerializeField] GameObject equipmentInventoryWindow;
        public EquipmentSlotType currentSelectedEquipmentSlot;
        [SerializeField] private GameObject equipmentInventorySlotPrefab;
        [SerializeField] private Transform equipmentInventoryContentWindow;
        [SerializeField] private Item currentSelectedItem;

        public void OpenEquipmentMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = true;
            menu.SetActive(true);
            equipmentInventoryWindow.SetActive(false);
            ClearEquipmentInventory();
            RefreshEquipmentSlotIcons();
        }

        public void RefreshMenu()
        {
            ClearEquipmentInventory();
            RefreshEquipmentSlotIcons();
        }

        public void SelectLastSelectedEquipmentSlot()
        {
            Button lastSelectedButton = null;

            switch (currentSelectedEquipmentSlot)
            {
                case EquipmentSlotType.RightWeapon01:
                    lastSelectedButton = rightHandSlot01.GetComponentInParent<Button>();
                    break;
                case EquipmentSlotType.RightWeapon02:
                    lastSelectedButton = rightHandSlot02.GetComponentInParent<Button>();
                    break;
                case EquipmentSlotType.RightWeapon03:
                    lastSelectedButton = rightHandSlot03.GetComponentInParent<Button>();
                    break;
                // ADD LOGIC FOR TALISMANS (Lumens)
                default:
                    break;
            }

            if (lastSelectedButton != null)
            {
                lastSelectedButton.Select();
                lastSelectedButton.OnSelect(null);
            }
        }

        public void CloseEquipmentMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

        private void RefreshEquipmentSlotIcons()
        {
            PlayerManager player = FindFirstObjectByType<PlayerManager>();

            if (player != null)
            {
                // -------------------- WEAPONS --------------------

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
            
                // -------------------- TALISMANS (Lumens) --------------------
            }
        }
    
        private void ClearEquipmentInventory()
        {
            foreach (Transform item in equipmentInventoryContentWindow)
            {
                Destroy(item.gameObject);
            }
        }

        public void LoadEquipmentInventory()
        {
            equipmentInventoryWindow.SetActive(true);

            switch (currentSelectedEquipmentSlot)
            {
                case EquipmentSlotType.RightWeapon01:
                    LoadWeaponInventory();
                    break;
                case EquipmentSlotType.RightWeapon02:
                    LoadWeaponInventory();
                    break;
                case EquipmentSlotType.RightWeapon03:
                    LoadWeaponInventory();
                    break;
                // ADD LOGIC FOR TALISMANS (Lumens)
                default:
                    break;
            }
        }

        private void LoadWeaponInventory()
        {
            List<WeaponItem> weaponsInInventory = new List<WeaponItem>();

            PlayerManager player = FindFirstObjectByType<PlayerManager>();

            if (player != null)
            {
                for(int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
                {
                    WeaponItem weapon = player.playerInventoryManager.itemsInInventory[i] as WeaponItem;

                    if (weapon != null)
                        weaponsInInventory.Add(weapon);
                }

                if (weaponsInInventory.Count <= 0)
                {
                    RefreshMenu();
                    return;
                }

                bool hasSelectedFirstInventorySlot = false;

                for (int i = 0; i < weaponsInInventory.Count; i++)
                {
                    GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                    UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                    equipmentInventorySlot.AddItem(weaponsInInventory[i]);

                    // THIS WILL SELECT THE FIRST BUTTON IN THE LIST
                    if (!hasSelectedFirstInventorySlot)
                    {
                        Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                        inventorySlotButton.Select();
                        inventorySlotButton.OnSelect(null);
                    }
                }
            }
        }
    
        public void SelectEquipmentSlot(int equipmentSlot)
        {
            currentSelectedEquipmentSlot = (EquipmentSlotType)equipmentSlot;
        }

        public void UnEquipSelectedItem()
        {
            PlayerManager player = FindFirstObjectByType<PlayerManager>();

            Item unequippedItem;
            switch (currentSelectedEquipmentSlot)
            {
                case EquipmentSlotType.RightWeapon01:
                    unequippedItem = player.playerInventoryManager.weaponsInRightHandSlots[0];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInRightHandSlots[0] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }

                    if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                        player.CurrentRightHandWeaponID = WorldItemDatabase.instance.unarmedWeapon.itemID;

                    break;
                case EquipmentSlotType.RightWeapon02:
                    unequippedItem = player.playerInventoryManager.weaponsInRightHandSlots[1];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInRightHandSlots[1] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }

                    if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                        player.CurrentRightHandWeaponID = WorldItemDatabase.instance.unarmedWeapon.itemID;
                    
                    break;
                case EquipmentSlotType.RightWeapon03:
                    unequippedItem = player.playerInventoryManager.weaponsInRightHandSlots[2];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.weaponsInRightHandSlots[2] = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                        if (unequippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                            player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }

                    if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                        player.CurrentRightHandWeaponID = WorldItemDatabase.instance.unarmedWeapon.itemID;
                    
                    break;
                // ADD LOGIC FOR TALISMANS (Lumens)
                default:
                    break;
            }

            // REFRESHES MENU
            RefreshMenu();
        }
    }
}
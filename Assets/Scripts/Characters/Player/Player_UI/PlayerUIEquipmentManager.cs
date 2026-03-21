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

        [Header("Lumen Slots")]
        [SerializeField] private Image lumenEquipmentSlot01;
        [SerializeField] private Image lumenEquipmentSlot02;
        [SerializeField] private Image lumenEquipmentSlot03;
        [SerializeField] private Image lumenEquipmentSlot04;

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
                // LOGIC FOR WEAPONS
                case EquipmentSlotType.RightWeapon01:
                    lastSelectedButton = rightHandSlot01.GetComponentInParent<Button>();
                    break;
                case EquipmentSlotType.RightWeapon02:
                    lastSelectedButton = rightHandSlot02.GetComponentInParent<Button>();
                    break;
                case EquipmentSlotType.RightWeapon03:
                    lastSelectedButton = rightHandSlot03.GetComponentInParent<Button>();
                    break;
                // LOGIC FOR TALISMANS (Lumens)
                case EquipmentSlotType.LumenEquipment01:
                    lastSelectedButton = lumenEquipmentSlot01.GetComponentInParent<Button>();
                    break;
                case EquipmentSlotType.LumenEquipment02:
                    lastSelectedButton = lumenEquipmentSlot02.GetComponentInParent<Button>();
                    break;
                case EquipmentSlotType.LumenEquipment03:
                    lastSelectedButton = lumenEquipmentSlot03.GetComponentInParent<Button>();
                    break;
                case EquipmentSlotType.LumenEquipment04:
                    lastSelectedButton = lumenEquipmentSlot04.GetComponentInParent<Button>();
                    break;
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

                // LUMEN 01
                LumenEquipmentItem lumenEquipmentItem01 = player.playerInventoryManager.lumenEquipmentItemSlots[0];

                if (lumenEquipmentItem01 != null)
                {
                    lumenEquipmentSlot01.enabled = true;
                    lumenEquipmentSlot01.sprite = lumenEquipmentItem01.itemIcon;
                }
                else
                {
                    lumenEquipmentSlot01.enabled = false;
                }

                // LUMEN 02
                LumenEquipmentItem lumenEquipmentItem02 = player.playerInventoryManager.lumenEquipmentItemSlots[1];

                if (lumenEquipmentItem02 != null)
                {
                    lumenEquipmentSlot02.enabled = true;
                    lumenEquipmentSlot02.sprite = lumenEquipmentItem02.itemIcon;
                }
                else
                {
                    lumenEquipmentSlot02.enabled = false;
                }

                // LUMEN 03
                LumenEquipmentItem lumenEquipmentItem03 = player.playerInventoryManager.lumenEquipmentItemSlots[2];

                if (lumenEquipmentItem03 != null)
                {
                    lumenEquipmentSlot03.enabled = true;
                    lumenEquipmentSlot03.sprite = lumenEquipmentItem03.itemIcon;
                }
                else
                {
                    lumenEquipmentSlot03.enabled = false;
                }

                // LUMEN 04
                LumenEquipmentItem lumenEquipmentItem04 = player.playerInventoryManager.lumenEquipmentItemSlots[3];

                if (lumenEquipmentItem04 != null)
                {
                    lumenEquipmentSlot04.enabled = true;
                    lumenEquipmentSlot04.sprite = lumenEquipmentItem04.itemIcon;
                }
                else
                {
                    lumenEquipmentSlot04.enabled = false;
                }
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
                // LOGIC FOR TALISMANS (Lumens)
                case EquipmentSlotType.LumenEquipment01:
                    LoadLumenInventory();
                    break;
                case EquipmentSlotType.LumenEquipment02:
                    LoadLumenInventory();
                    break;
                case EquipmentSlotType.LumenEquipment03:
                    LoadLumenInventory();
                    break;
                case EquipmentSlotType.LumenEquipment04:
                    LoadLumenInventory();
                    break;
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

        private void LoadLumenInventory()
        {
            List<LumenEquipmentItem> lumensInInventory = new List<LumenEquipmentItem>();

            PlayerManager player = FindFirstObjectByType<PlayerManager>();

            if (player != null)
            {
                for(int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
                {
                    LumenEquipmentItem lumen = player.playerInventoryManager.itemsInInventory[i] as LumenEquipmentItem;

                    if (lumen != null)
                        lumensInInventory.Add(lumen);
                }

                if (lumensInInventory.Count <= 0)
                {
                    RefreshMenu();
                    return;
                }

                bool hasSelectedFirstInventorySlot = false;

                for (int i = 0; i < lumensInInventory.Count; i++)
                {
                    GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                    UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                    equipmentInventorySlot.AddItem(lumensInInventory[i]);

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
                // LOGIC FOR WEAPONS
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
                // LOGIC FOR TALISMANS (Lumens)
                case EquipmentSlotType.LumenEquipment01:
                    unequippedItem = player.playerInventoryManager.lumenEquipmentItemSlots[0];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }

                    player.playerInventoryManager.lumenEquipmentItemSlots[0] = null;
                    player.playerEquipmentManager.LoadLumenEquipment(player.playerInventoryManager.lumenEquipmentItemSlots[0], 0);
                    player.SetLumenEquipmentID(0, -1);
                    
                    break;
                case EquipmentSlotType.LumenEquipment02:
                    unequippedItem = player.playerInventoryManager.lumenEquipmentItemSlots[1];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }

                    player.playerInventoryManager.lumenEquipmentItemSlots[1] = null;
                    player.playerEquipmentManager.LoadLumenEquipment(player.playerInventoryManager.lumenEquipmentItemSlots[1], 1);
                    player.SetLumenEquipmentID(1, -1);
                    
                    break;
                case EquipmentSlotType.LumenEquipment03:
                    unequippedItem = player.playerInventoryManager.lumenEquipmentItemSlots[2];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }

                    player.playerInventoryManager.lumenEquipmentItemSlots[2] = null;
                    player.playerEquipmentManager.LoadLumenEquipment(player.playerInventoryManager.lumenEquipmentItemSlots[2], 2);
                    player.SetLumenEquipmentID(2, -1);
                    
                    break;
                case EquipmentSlotType.LumenEquipment04:
                    unequippedItem = player.playerInventoryManager.lumenEquipmentItemSlots[3];
                    if (unequippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    }

                    player.playerInventoryManager.lumenEquipmentItemSlots[3] = null;
                    player.playerEquipmentManager.LoadLumenEquipment(player.playerInventoryManager.lumenEquipmentItemSlots[3], 3);
                    player.SetLumenEquipmentID(3, -1);
                    
                    break;
                default:
                    break;
            }

            // REFRESHES MENU
            RefreshMenu();
        }
    }
}
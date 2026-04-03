using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Group1
{
    public class PlayerUIEquipmentManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] private GameObject menu;

        [Header("Weapon Slots")]
        [SerializeField] private Image rightHandSlot01;
        private Button rightHandSlot01Button;
        [SerializeField] private Image rightHandSlot02;
        private Button rightHandSlot02Button;
        [SerializeField] private Image rightHandSlot03;
        private Button rightHandSlot03Button;

        [Header("Quick Slots")]
        [SerializeField] private Image quickSlot01;
        [SerializeField] private TextMeshProUGUI quickSlot01Count;
        private Button quickSlot01Button;
        [SerializeField] private Image quickSlot02;
        [SerializeField] private TextMeshProUGUI quickSlot02Count;
        private Button quickSlot02Button;
        [SerializeField] private Image quickSlot03;
        [SerializeField] private TextMeshProUGUI quickSlot03Count;
        private Button quickSlot03Button;

        // THIS INVENTORY POPULATES WITH RELATED ITEMS WHEN CHANGING EQUIPMENT
        [Header("Equipment Inventory")]
        [SerializeField] GameObject equipmentInventoryWindow;
        public EquipmentSlotType currentSelectedEquipmentSlot;
        [SerializeField] private GameObject equipmentInventorySlotPrefab;
        [SerializeField] private Transform equipmentInventoryContentWindow;
        [SerializeField] private Item currentSelectedItem;

        private void Awake()
        {
            // WEAPON SLOT BUTTONS
            rightHandSlot01Button = rightHandSlot01.GetComponentInParent<Button>(true);
            rightHandSlot02Button = rightHandSlot02.GetComponentInParent<Button>(true);
            rightHandSlot03Button = rightHandSlot03.GetComponentInParent<Button>(true);

            // QUICK SLOT BUTTONS
            quickSlot01Button = quickSlot01.GetComponentInParent<Button>(true);
            quickSlot02Button = quickSlot02.GetComponentInParent<Button>(true);
            quickSlot03Button = quickSlot03.GetComponentInParent<Button>(true);
        }

        public void OpenEquipmentMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = true;
            ToggleEquipmentButtons(true);
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

        private void ToggleEquipmentButtons(bool isEnabled)
        {
            rightHandSlot01Button.enabled = isEnabled;
            rightHandSlot02Button.enabled = isEnabled;
            rightHandSlot03Button.enabled = isEnabled;

            quickSlot01Button.enabled = isEnabled;
            quickSlot02Button.enabled = isEnabled;
            quickSlot03Button.enabled = isEnabled;
        }

        public void SelectLastSelectedEquipmentSlot()
        {
            Button lastSelectedButton = null;

            switch (currentSelectedEquipmentSlot)
            {
                // LOGIC FOR WEAPONS
                case EquipmentSlotType.RightWeapon01:
                    lastSelectedButton = rightHandSlot01Button;
                    break;
                case EquipmentSlotType.RightWeapon02:
                    lastSelectedButton = rightHandSlot02Button;
                    break;
                case EquipmentSlotType.RightWeapon03:
                    lastSelectedButton = rightHandSlot03Button;
                    break;
                // LOGIC FOR TALISMANS (Lumens)
                // LOGIC FOR QUICK SLOTS
                case EquipmentSlotType.QuickSlot01:
                    lastSelectedButton = quickSlot01Button;
                    break;
                case EquipmentSlotType.QuickSlot02:
                    lastSelectedButton = quickSlot02Button;
                    break;
                case EquipmentSlotType.QuickSlot03:
                    lastSelectedButton = quickSlot03Button;
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

                // -------------------- QUICK SLOTS --------------------

                // QUICK SLOT 01
                QuickSlotItem quickSlot01Equipment = player.playerInventoryManager.quickSlotItemsInQuickSlots[0];

                if (quickSlot01Equipment != null)
                {
                    quickSlot01.enabled = true;
                    quickSlot01.sprite = quickSlot01Equipment.itemIcon;

                    if (quickSlot01Equipment.isConsumable)
                    {
                        quickSlot01Count.enabled = true;
                        quickSlot01Count.text = quickSlot01Equipment.GetCurrentAmount(player).ToString();
                    }
                    else
                    {
                        quickSlot01Count.enabled = false;
                    }
                }
                else
                {
                    quickSlot01.enabled = false;
                    quickSlot01Count.enabled = false;
                }

                // QUICK SLOT 02
                QuickSlotItem quickSlot02Equipment = player.playerInventoryManager.quickSlotItemsInQuickSlots[1];

                if (quickSlot02Equipment != null)
                {
                    quickSlot02.enabled = true;
                    quickSlot02.sprite = quickSlot02Equipment.itemIcon;

                    if (quickSlot02Equipment.isConsumable)
                    {
                        quickSlot02Count.enabled = true;
                        quickSlot02Count.text = quickSlot02Equipment.GetCurrentAmount(player).ToString();
                    }
                    else
                    {
                        quickSlot02Count.enabled = false;
                    }
                }
                else
                {
                    quickSlot02.enabled = false;
                    quickSlot02Count.enabled = false;
                }
            
                // QUICK SLOT 03
                QuickSlotItem quickSlot03Equipment = player.playerInventoryManager.quickSlotItemsInQuickSlots[2];

                if (quickSlot03Equipment != null)
                {
                    quickSlot03.enabled = true;
                    quickSlot03.sprite = quickSlot03Equipment.itemIcon;

                    if (quickSlot03Equipment.isConsumable)
                    {
                        quickSlot03Count.enabled = true;
                        quickSlot03Count.text = quickSlot03Equipment.GetCurrentAmount(player).ToString();
                    }
                    else
                    {
                        quickSlot03Count.enabled = false;
                    }
                }
                else
                {
                    quickSlot03.enabled = false;
                    quickSlot03Count.enabled = false;
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
                // LOGIC FOR QUICK SLOTS
                case EquipmentSlotType.QuickSlot01:
                    LoadQuickSlotInventory();
                    break;
                case EquipmentSlotType.QuickSlot02:
                    LoadQuickSlotInventory();
                    break;
                case EquipmentSlotType.QuickSlot03:
                    LoadQuickSlotInventory();
                    break;
                default:
                    break;
            }
        }

        // LOGIC FOR LOADING ALL UNEQUIPPED WEAPONS IN INVENTORY

        private void LoadWeaponInventory()
        {
            PlayerManager player = FindFirstObjectByType<PlayerManager>();

            List<WeaponItem> weaponsInInventory = new List<WeaponItem>();

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

        private void LoadQuickSlotInventory()
        {
            PlayerManager player = FindFirstObjectByType<PlayerManager>();

            List<QuickSlotItem> quickSlotItemsInInventory = new List<QuickSlotItem>();

            if (player != null)
            {
                for(int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
                {
                    QuickSlotItem item = player.playerInventoryManager.itemsInInventory[i] as QuickSlotItem;

                    if (item != null)
                        quickSlotItemsInInventory.Add(item);
                }

                if (quickSlotItemsInInventory.Count <= 0)
                {
                    RefreshMenu();
                    return;
                }

                bool hasSelectedFirstInventorySlot = false;

                for (int i = 0; i < quickSlotItemsInInventory.Count; i++)
                {
                    GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
                    UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                    equipmentInventorySlot.AddItem(quickSlotItemsInInventory[i]);

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
                // LOGIC FOR QUICK SLOTS
                case EquipmentSlotType.QuickSlot01:
                    unequippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[0];
                    if (unequippedItem != null)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[0] = null;
                    
                    if (player.playerInventoryManager.quickSlotItemIndex == 0)
                        player.CurrentQuickSlotItemID = -1;

                    break;
                case EquipmentSlotType.QuickSlot02:
                    unequippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[1];
                    if (unequippedItem != null)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[1] = null;
                    
                    if (player.playerInventoryManager.quickSlotItemIndex == 1)
                        player.CurrentQuickSlotItemID = -1;

                    break;
                case EquipmentSlotType.QuickSlot03:
                    unequippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[2];
                    if (unequippedItem != null)
                        player.playerInventoryManager.AddItemToInventory(unequippedItem);
                    
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[2] = null;
                    
                    if (player.playerInventoryManager.quickSlotItemIndex == 2)
                        player.CurrentQuickSlotItemID = -1;

                    break;
                default:
                    break;
            }

            // REFRESHES MENU
            RefreshMenu();
        }
    }
}
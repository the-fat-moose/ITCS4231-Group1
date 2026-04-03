using UnityEngine;
using UnityEngine.UI;

namespace Group1
{
    public class UI_EquipmentInventorySlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightedIcon;
        [SerializeField] public Item currentItem;

        public void AddItem(Item item)
        {
            if (item == null)
            {
                itemIcon.enabled = false;
                return;
            }

            itemIcon.enabled = true;

            currentItem = item;
            itemIcon.sprite = item.itemIcon;
        }

        public void SelectSlot()
        {
            highlightedIcon.enabled = true;
        }

        public void DeselectSlot()
        {
            highlightedIcon.enabled = false;
        }

        public void EquipItem()
        {
            PlayerManager player = FindFirstObjectByType<PlayerManager>();
            Item equippedItem;

            if (player != null) 
            {
                switch (PlayerUIManager.instance.playerUIEquipmentManager.currentSelectedEquipmentSlot)
                {
                    // LOGIC FOR WEAPONS
                    case EquipmentSlotType.RightWeapon01:
                        equippedItem = player.playerInventoryManager.weaponsInRightHandSlots[0];
                        
                        // IF OUR CURRENT WEAPON IN THIS SLOT IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                        if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(equippedItem);
                        }

                        // THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR NEW WEAPON
                        player.playerInventoryManager.weaponsInRightHandSlots[0] = currentItem as WeaponItem;

                        // THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                        player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                        // RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT
                        if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                            player.CurrentRightHandWeaponID = currentItem.itemID;

                        // REFRESHES EQUIPMENT WINDOW
                        PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                        break;
                    case EquipmentSlotType.RightWeapon02:
                        equippedItem = player.playerInventoryManager.weaponsInRightHandSlots[1];
                        
                        // IF OUR CURRENT WEAPON IN THIS SLOT IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                        if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(equippedItem);
                        }

                        // THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR NEW WEAPON
                        player.playerInventoryManager.weaponsInRightHandSlots[1] = currentItem as WeaponItem;

                        // THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                        player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                        // RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT
                        if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                            player.CurrentRightHandWeaponID = currentItem.itemID;

                        // REFRESHES EQUIPMENT WINDOW
                        PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                        break;
                    case EquipmentSlotType.RightWeapon03:
                        equippedItem = player.playerInventoryManager.weaponsInRightHandSlots[2];
                        
                        // IF OUR CURRENT WEAPON IN THIS SLOT IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                        if (equippedItem.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(equippedItem);
                        }

                        // THEN REPLACE THE WEAPON IN THAT SLOT WITH OUR NEW WEAPON
                        player.playerInventoryManager.weaponsInRightHandSlots[2] = currentItem as WeaponItem;

                        // THEN REMOVE THE NEW WEAPON FROM OUR INVENTORY
                        player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                        // RE-EQUIP NEW WEAPON IF WE ARE HOLDING THE CURRENT WEAPON IN THIS SLOT
                        if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                            player.CurrentRightHandWeaponID = currentItem.itemID;

                        // REFRESHES EQUIPMENT WINDOW
                        PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                        break;
                    // LOGIC FOR TALISMANS (Lumens)
                    // LOGIC FOR QUICK SLOTS
                    case EquipmentSlotType.QuickSlot01:
                        equippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[0];

                        // IF OUR CURRENT ITEM IN THIS SLOT IS NOT NULL, ADD IT TO OUR INVENTORY
                        if (equippedItem != null)
                        {
                            player.playerInventoryManager.AddItemToInventory(equippedItem);
                        }

                        // THEN REPLACE THE ITEM IN THAT SLOT WITH OUR NEW ITEM
                        player.playerInventoryManager.quickSlotItemsInQuickSlots[0] = currentItem as QuickSlotItem;

                        // THEN REMOVE THE NEW ITEM FROM OUR INVENTORY
                        player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                        // RE-EQUIP THE NEW ITEM
                        if (player.playerInventoryManager.quickSlotItemIndex == 0)
                            player.CurrentQuickSlotItemID = currentItem.itemID;

                        // REFRESHES EQUIPMENT WINDOW
                        PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                        break;
                    case EquipmentSlotType.QuickSlot02:
                        equippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[1];

                        // IF OUR CURRENT ITEM IN THIS SLOT IS NOT NULL, ADD IT TO OUR INVENTORY
                        if (equippedItem != null)
                        {
                            player.playerInventoryManager.AddItemToInventory(equippedItem);
                        }

                        // THEN REPLACE THE ITEM IN THAT SLOT WITH OUR NEW ITEM
                        player.playerInventoryManager.quickSlotItemsInQuickSlots[1] = currentItem as QuickSlotItem;

                        // THEN REMOVE THE NEW ITEM FROM OUR INVENTORY
                        player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                        // RE-EQUIP THE NEW ITEM
                        if (player.playerInventoryManager.quickSlotItemIndex == 1)
                            player.CurrentQuickSlotItemID = currentItem.itemID;

                        // REFRESHES EQUIPMENT WINDOW
                        PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                        break;
                    case EquipmentSlotType.QuickSlot03:
                        equippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[2];

                        // IF OUR CURRENT ITEM IN THIS SLOT IS NOT NULL, ADD IT TO OUR INVENTORY
                        if (equippedItem != null)
                        {
                            player.playerInventoryManager.AddItemToInventory(equippedItem);
                        }

                        // THEN REPLACE THE ITEM IN THAT SLOT WITH OUR NEW ITEM
                        player.playerInventoryManager.quickSlotItemsInQuickSlots[2] = currentItem as QuickSlotItem;

                        // THEN REMOVE THE NEW ITEM FROM OUR INVENTORY
                        player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                        // RE-EQUIP THE NEW ITEM
                        if (player.playerInventoryManager.quickSlotItemIndex == 2)
                            player.CurrentQuickSlotItemID = currentItem.itemID;

                        // REFRESHES EQUIPMENT WINDOW
                        PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                        break;
                    default:
                        break;
                }

                PlayerUIManager.instance.playerUIEquipmentManager.SelectLastSelectedEquipmentSlot();
            }
        }
    }
}
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
                    case EquipmentSlotType.LumenEquipment01:
                        equippedItem = player.playerInventoryManager.lumenEquipmentItemSlots[0];

                        // IF OUR CURRENT LUMEN IN THIS SLOT IS NOT NULL, ADD IT TO OUR INVENTORY
                        if (equippedItem != null)
                        {
                            player.playerInventoryManager.AddItemToInventory(equippedItem);
                        }

                        // THEN REPLACE THE LUMEN IN THAT SLOT WITH OUR NEW LUMEN
                        player.playerInventoryManager.lumenEquipmentItemSlots[0] = equippedItem as LumenEquipmentItem;
                        player.SetLumenEquipmentID(0, player.playerInventoryManager.lumenEquipmentItemSlots[0].itemID);

                        // EQUIP NEW LUMEN
                        //player.playerEquipmentManager.LoadLumenEquipment(player.playerInventoryManager.lumenEquipmentItemSlots[0], 0);

                        // REFRESHES EQUIPMENT WINDOW
                        PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                        break;
                    case EquipmentSlotType.LumenEquipment02:
                        equippedItem = player.playerInventoryManager.lumenEquipmentItemSlots[1];

                        // IF OUR CURRENT LUMEN IN THIS SLOT IS NOT NULL, ADD IT TO OUR INVENTORY
                        if (equippedItem != null)
                        {
                            player.playerInventoryManager.AddItemToInventory(equippedItem);
                        }

                        // THEN REPLACE THE LUMEN IN THAT SLOT WITH OUR NEW LUMEN
                        player.playerInventoryManager.lumenEquipmentItemSlots[1] = equippedItem as LumenEquipmentItem;
                        player.SetLumenEquipmentID(1, player.playerInventoryManager.lumenEquipmentItemSlots[1].itemID);

                        // EQUIP NEW LUMEN
                        //player.playerEquipmentManager.LoadLumenEquipment(player.playerInventoryManager.lumenEquipmentItemSlots[1], 1);

                        // REFRESHES EQUIPMENT WINDOW
                        PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                        break;
                    case EquipmentSlotType.LumenEquipment03:
                        equippedItem = player.playerInventoryManager.lumenEquipmentItemSlots[2];

                        // IF OUR CURRENT LUMEN IN THIS SLOT IS NOT NULL, ADD IT TO OUR INVENTORY
                        if (equippedItem != null)
                        {
                            player.playerInventoryManager.AddItemToInventory(equippedItem);
                        }

                        // THEN REPLACE THE LUMEN IN THAT SLOT WITH OUR NEW LUMEN
                        player.playerInventoryManager.lumenEquipmentItemSlots[2] = equippedItem as LumenEquipmentItem;
                        player.SetLumenEquipmentID(2, player.playerInventoryManager.lumenEquipmentItemSlots[2].itemID);

                        // EQUIP NEW LUMEN
                        //player.playerEquipmentManager.LoadLumenEquipment(player.playerInventoryManager.lumenEquipmentItemSlots[2], 2);

                        // REFRESHES EQUIPMENT WINDOW
                        PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();

                        break;
                    case EquipmentSlotType.LumenEquipment04:
                        equippedItem = player.playerInventoryManager.lumenEquipmentItemSlots[3];

                        // IF OUR CURRENT LUMEN IN THIS SLOT IS NOT NULL, ADD IT TO OUR INVENTORY
                        if (equippedItem != null)
                        {
                            player.playerInventoryManager.AddItemToInventory(equippedItem);
                        }

                        // THEN REPLACE THE LUMEN IN THAT SLOT WITH OUR NEW LUMEN
                        player.playerInventoryManager.lumenEquipmentItemSlots[3] = equippedItem as LumenEquipmentItem;
                        player.SetLumenEquipmentID(3, player.playerInventoryManager.lumenEquipmentItemSlots[3].itemID);

                        // EQUIP NEW LUMEN
                        //player.playerEquipmentManager.LoadLumenEquipment(player.playerInventoryManager.lumenEquipmentItemSlots[3], 3);

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
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
            WeaponItem currentWeapon;

            if (player != null) 
            {
                switch (PlayerUIManager.instance.playerUIEquipmentManager.currentSelectedEquipmentSlot)
                {
                    case EquipmentSlotType.RightWeapon01:
                        currentWeapon = player.playerInventoryManager.weaponsInRightHandSlots[0];
                        
                        // IF OUR CURRENT WEAPON IN THIS SLOT IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                        if (currentWeapon.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(currentWeapon);
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
                        currentWeapon = player.playerInventoryManager.weaponsInRightHandSlots[1];
                        
                        // IF OUR CURRENT WEAPON IN THIS SLOT IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                        if (currentWeapon.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(currentWeapon);
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
                        currentWeapon = player.playerInventoryManager.weaponsInRightHandSlots[2];
                        
                        // IF OUR CURRENT WEAPON IN THIS SLOT IS NOT AN UNARMED ITEM, ADD IT TO OUR INVENTORY
                        if (currentWeapon.itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                        {
                            player.playerInventoryManager.AddItemToInventory(currentWeapon);
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
                    // ADD LOGIC FOR TALISMANS (Lumens)
                    default:
                        break;
                }

                PlayerUIManager.instance.playerUIEquipmentManager.SelectLastSelectedEquipmentSlot();
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Group1 {
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        [Header("Weapons")]
        public WeaponItem currentRightHandWeapon;

        [Header("Quick Slots")]
        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
        public int rightHandWeaponIndex = 0;
        public QuickSlotItem[] quickSlotItemsInQuickSlots = new QuickSlotItem[3];
        public int quickSlotItemIndex = 0;
        public QuickSlotItem currentQuickSlotItem;

        [Header("Lumens")]
        public LumenItem lumenSlot1Item;
        public LumenItem lumenSlot2Item;
        public LumenItem lumenSlot3Item;
        public LumenItem lumenSlot4Item;

        [Header("Player Abilities")]
        public PlayerAbilityAction pushAbility;
        public PlayerAbilityAction knockUpAbility;
        public PlayerAbilityAction cageAbility;

        [Header("Inventory")]
        public List<Item> itemsInInventory;

        public void AddItemToInventory(Item item)
        {
            itemsInInventory.Add(item);
        }

        public void RemoveItemFromInventory(Item item)
        {
            itemsInInventory.Remove(item);

            // CHECK FOR NULL LIST SLOTS AND REMOVE THEM
            for (int i = itemsInInventory.Count - 1; i > -1; i--)
            {
                if (itemsInInventory[i] == null)
                {
                    itemsInInventory.RemoveAt(i);
                }
            }
        }
    }
}
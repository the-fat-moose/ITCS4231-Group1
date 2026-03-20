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

        public PlayerAbilityAction pushAbility;
        public PlayerAbilityAction knockUpAbility;
        public PlayerAbilityAction cageAbility;

        [Header("Lumens")]
        public LumenEquipmentItem[] lumenEquipmentItemSlots = new LumenEquipmentItem[4];

        [Header("Inventory")]
        public List<Item> itemsInInventory;

        public void AddItemToInventory(Item item)
        {
            itemsInInventory.Add(item);
        }

        public void RemoveItemFromInventory()
        {
            
        }
    }
}
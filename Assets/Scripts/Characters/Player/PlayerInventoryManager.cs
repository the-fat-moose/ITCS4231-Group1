using UnityEngine;

namespace Group1 {
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public WeaponItem currentRightHandWeapon;

        [Header("Quick Slots")]
        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
        public int rightHandWeaponIndex = 0;
    }
}
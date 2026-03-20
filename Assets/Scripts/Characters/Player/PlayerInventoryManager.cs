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
    }
}
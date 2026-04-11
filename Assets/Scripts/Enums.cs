using UnityEngine;

namespace Group1 {
    public class Enums : MonoBehaviour
    {
        
    }

    public enum CharacterSlot
    {
        CharacterSlot_01,
        CharacterSlot_02,
        CharacterSlot_03,
        CharacterSlot_04,
        CharacterSlot_05,
        CharacterSlot_06,
        CharacterSlot_07,
        CharacterSlot_08,
        CharacterSlot_09,
        CharacterSlot_10,
        NO_SLOT
    }

    public enum CharacterGroup
    {
        Team01, // Friendly (Player)
        Team02 // Enemy AI
    }

    public enum WeaponModelSlot
    {
        RightHand
    }

    public enum EquipmentModelType
    {
        Lumen
    }

    public enum EquipmentSlotType
    {
        RightWeapon01,    // 0
        RightWeapon02,    // 1
        RightWeapon03,    // 2
        QuickSlot01,      // 3
        QuickSlot02,      // 4
        QuickSlot03,      // 5
        LumenEquipment01, // 6
        LumenEquipment02, // 7
        LumenEquipment03, // 8
        LumenEquipment04  // 9
    }

    public enum AttackType
    {
        Light01,
        Light02,
        Light03,
        Heavy01, 
        Heavy02, 
        PushAbility,
        KnockUpAbility,
        CageAbility,
        AOEAbility
    }

    public enum ItemPickUpType
    {
        WorldSpawn,
        CharacterDrop
    }
}
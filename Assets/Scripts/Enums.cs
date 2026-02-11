using UnityEngine;

namespace Group1 {
    public class Enums : MonoBehaviour
    {
        
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

    public enum AttackType
    {
        Light,
        Heavy
    }
}
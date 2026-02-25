using UnityEngine;

namespace Group1 {
    [System.Serializable]
    // SINCE WE WANT TO REFERENCE THIS DATA FOR EVERY SAVE FILE, THIS SCRIPT IS NOT A MONOBEHAVIOR AND IS INSTEAD SERIALIZABLE
    public class CharacterSaveData
    {
        [Header("Character Name")]
        public string characterName = "Character";

        [Header("Time Played")]
        public float secondsPlayed;
        
        // WE CAN ONLY SAVE DATA FROM BASIC VARIABLE TYPES, SO WE CANT USE A VECTOR3 (float, int, bool, string, etc.)
        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;
    }
}
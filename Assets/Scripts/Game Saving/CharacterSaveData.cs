using UnityEngine;

namespace Group1 {
    [System.Serializable]
    // SINCE WE WANT TO REFERENCE THIS DATA FOR EVERY SAVE FILE, THIS SCRIPT IS NOT A MONOBEHAVIOR AND IS INSTEAD SERIALIZABLE
    public class CharacterSaveData
    {
        [Header("Scene Index")]
        public int sceneIndex = 1; // Defaulted to the first level

        [Header("Character Name")]
        public string characterName = "Character";

        [Header("Time Played")]
        public float secondsPlayed;
        
        // WE CAN ONLY SAVE DATA FROM BASIC VARIABLE TYPES, SO WE CANT USE A VECTOR3 (float, int, bool, string, etc.)
        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;
        public int currentMana = -1; // Defaulted to -1 because mana cannot be lower than 0, so if this is read as less than 0, then the character is set to their max mana

        [Header("Stats")]
        public int vitality;
        public int endurance;
        public int mind;
    }
}
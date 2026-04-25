using System.Collections.Generic;
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
        public int currentMana;

        [Header("Stats")]
        public int vitality;
        public int endurance;
        public int mind;

        [Header("Doors")]
        public List<string> doorsOpened;

        [Header("Geodes")]
        public int lastGeodeRestedAt = 0;
        public SerializableDictionary<int, bool> geodes; // THE INT IS THE GEODE I.D, THE BOOL IS THE ACTIVATED STATUS

        [Header("Bosses")]
        public SerializableDictionary<int, bool> bossesAwakened; // THE INT IS THE BOSS I.D, THE BOOL IS THE AWAKENED STATUS
        public SerializableDictionary<int, bool> bossesDefeated; // THE INT IS THE BOSS I.D, THE BOOL IS THE DEFEATED STATUS

        [Header("World Items")]
        public SerializableDictionary<int, bool> worldItemsLooted; // THE INT IS THE ITEM I.D, THE BOOL IS THE LOOTED STATUS

        [Header("Equipment")]
        public int rightWeaponIndex;
        public SerializableWeapon rightWeapon01;
        public SerializableWeapon rightWeapon02;
        public SerializableWeapon rightWeapon03;

        public int quickSlotItemIndex;
        public SerializableQuickSlotItem quickSlotItem01;
        public SerializableQuickSlotItem quickSlotItem02;
        public SerializableQuickSlotItem quickSlotItem03;

        public int lumen01;
        public int lumen02;
        public int lumen03;
        public int lumen04;

        public int currentHealthFlasksRemaining = 5;

        [Header("Inventory")]
        public List<SerializableWeapon> weaponsInInventory;
        public List<SerializableQuickSlotItem> quickSlotItemsInInventory;
        public List<int> lumenEquipmentInInventory;

        public CharacterSaveData()
        {
            doorsOpened = new List<string>();
            
            geodes = new SerializableDictionary<int, bool>();
            bossesAwakened = new SerializableDictionary<int, bool>();
            bossesDefeated = new SerializableDictionary<int, bool>();
            worldItemsLooted = new SerializableDictionary<int, bool>();

            weaponsInInventory = new List<SerializableWeapon>();
            quickSlotItemsInInventory = new List<SerializableQuickSlotItem>();
            lumenEquipmentInInventory = new List<int>();
        }
    }
}
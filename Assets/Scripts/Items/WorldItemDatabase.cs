using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Group1 {
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase instance;

        public WeaponItem unarmedWeapon;

        [Header("Weapons")]
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

        [Header("Lumen Equipment")]
        [SerializeField] List<LumenEquipmentItem> lumenEquipmentItems = new List<LumenEquipmentItem>();

        // A LIST OF EVERY ITEM WE HAVE IN THE GAME
        [Header("Items")]
        private List<Item> items = new List<Item>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            // ADD ALL OF OUR WEAPONS TO THE LIST OF ITEMS 
            foreach (var weapon in weapons)
            {
                items.Add(weapon);
            }

            foreach (var item in lumenEquipmentItems)
            {
                items.Add(item);
            }

            // ASSIGN ALL OF OUR ITEMS A UNIQUE ITEM ID 
            // (IF YOU CONSISTENTLY ADD NEW ITEMS, THE IDS WILL CHANGE AND THIS COULD BE BAD FOR SAVE FILES)
            for(int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }
    
        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }

        public LumenEquipmentItem GetLumenEquipmentByID(int ID)
        {
            return lumenEquipmentItems.FirstOrDefault(equipment => equipment.itemID == ID);
        }
    }
}
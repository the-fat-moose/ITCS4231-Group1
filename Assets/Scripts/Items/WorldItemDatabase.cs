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

        [Header("Quick Slot Items")]
        [SerializeField] List<QuickSlotItem> quickSlotItems = new List<QuickSlotItem>();

        [Header("Lumen Items")]
        [SerializeField] List<LumenItem> lumenItems = new List<LumenItem>();

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
            // ADD ALL OF OUR QUICK SLOT ITEMS TO THE LIST OF ITEMS 
            foreach (var quickSlotItem in quickSlotItems)
            {
                items.Add(quickSlotItem);
            }
            // ADD ALL OF OUR LUMEN ITEMS TO THE LIST OF ITEMS 
            foreach (var lumen in lumenItems)
            {
                items.Add(lumen);
            }            

            // ASSIGN ALL OF OUR ITEMS A UNIQUE ITEM ID 
            // (IF YOU CONSISTENTLY ADD NEW ITEMS, THE IDS WILL CHANGE AND THIS COULD BE BAD FOR SAVE FILES)
            for(int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }
    
        // ITEM DATA BASE

        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }

        public QuickSlotItem GetQuickSlotItemByID(int ID)
        {
            return quickSlotItems.FirstOrDefault(item => item.itemID == ID);
        }

        public LumenItem GetLumenItemByID(int ID)
        {
            return lumenItems.FirstOrDefault(item => item.itemID == ID);
        }
    
        // ITEM SERIALIZATION

        public WeaponItem GetWeaponFromSerializedData(SerializableWeapon serializableWeapon)
        {
            WeaponItem weapon = null;
            
            if (GetWeaponByID(serializableWeapon.itemID)) weapon = Instantiate(GetWeaponByID(serializableWeapon.itemID));

            if (weapon == null) return Instantiate(unarmedWeapon);

            return weapon;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Group1 {
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        [Header("Weapon Model Instantiation Slot")]
        public WeaponModelInstantiationSlot rightHandSlot;

        [Header("Weapon Managers")]
        public WeaponManager rightWeaponManager;

        [Header("Weapon Models")]
        public GameObject rightHandWeaponModel;

        [Header("DEBUG DELETE LATER")]
        [SerializeField] bool equipNewItems = false;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            // GET OUR WEAPON SLOT
            InitializeWeaponSlot();
        }

        protected override void Start()
        {
            base.Start();

            LoadWeaponInHand();
        }

        private void Update()
        {
            if (equipNewItems)
            {
                equipNewItems = false;

                EquipNewLumenItems();
            }
        }

        public void EquipNewLumenItems()
        {
            LoadLumenSlot1Equipment(player.playerInventoryManager.lumenSlot1Item);
            LoadLumenSlot2Equipment(player.playerInventoryManager.lumenSlot2Item);
            LoadLumenSlot3Equipment(player.playerInventoryManager.lumenSlot3Item);
            LoadLumenSlot4Equipment(player.playerInventoryManager.lumenSlot4Item);
        }

        // QUICK SLOTS
        public void SwitchQuickSlotItem()
        {
            QuickSlotItem selectedItem = null;
            
            player.playerInventoryManager.quickSlotItemIndex += 1;

            // IF OUR INDEX IS OUT OF BOUNDS, RESET IT TO POSITION #1 (index 0)
            if (player.playerInventoryManager.quickSlotItemIndex < 0 || 
                player.playerInventoryManager.quickSlotItemIndex > player.playerInventoryManager.quickSlotItemsInQuickSlots.Length - 1)
            {
                player.playerInventoryManager.quickSlotItemIndex = 0;

                float itemCount = 0;
                QuickSlotItem firstItem = null;
                int firstItemPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.quickSlotItemsInQuickSlots.Length; i++)
                {
                    if (player.playerInventoryManager.quickSlotItemsInQuickSlots[i] != null)
                    {
                        itemCount += 1;

                        if (firstItem == null)
                        {
                            firstItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[i];
                            firstItemPosition = i;
                        }
                    }
                }
                
                if (itemCount <= 1)
                {
                    player.playerInventoryManager.quickSlotItemIndex = -1;
                    selectedItem = null;

                    player.CurrentQuickSlotItemID = -1;
                }
                else
                {
                    player.playerInventoryManager.quickSlotItemIndex = firstItemPosition;

                    player.CurrentQuickSlotItemID = firstItem.itemID;
                }

                return;
            }

            if (player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex] != null)
            {
                selectedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex];

                player.CurrentQuickSlotItemID = player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex].itemID;
            }
            else
            {
                player.CurrentQuickSlotItemID = -1;
            }

            if (selectedItem == null && player.playerInventoryManager.quickSlotItemIndex <= player.playerInventoryManager.quickSlotItemsInQuickSlots.Length - 1)
            {
                SwitchQuickSlotItem();
            }
        }

        public void LoadQuickSlotEquipment(QuickSlotItem equipment)
        {
            if (equipment == null)
            {
                player.CurrentQuickSlotItemID = -1;
                player.playerInventoryManager.currentQuickSlotItem = null;

                return;
            }

            player.playerInventoryManager.currentQuickSlotItem = equipment;
            player.CurrentQuickSlotItemID = equipment.itemID;
        }

        // EQUIPMENT
        public void LoadLumenSlot1Equipment(LumenItem equipment)
        {
            // IF EQUIPMENT IS NULL, SIMPLY SET EQUIPMENT IN INVENTORY TO NULL AND RETURN
            if (equipment == null)
            {
                player.LumenSlot01EquipmentID = -1; // -1 WILL NEVER BE AN ITEM ID

                player.playerInventoryManager.lumenSlot1Item = null;

                // CALCULATE ALL STAT CHANGES
                LoadLumenSlot();

                return;
            }

            // IF YOU HAVE AN "OnItemEquipped" CALL ON YOUR EQUIPMENT, RUN IT HERE

            // SET CURRENT LUMEN EQUIPMENT IN PLAYER INVENTORY TO THE EQUIPMENT THAT IS PASSED TO THIS FUNCTION
            player.playerInventoryManager.lumenSlot1Item = equipment;

            // CALCULATE ALL STAT CHANGES
            LoadLumenSlot();

            player.LumenSlot01EquipmentID = equipment.itemID;
        }

        public void LoadLumenSlot2Equipment(LumenItem equipment)
        {
            // IF EQUIPMENT IS NULL, SIMPLY SET EQUIPMENT IN INVENTORY TO NULL AND RETURN
            if (equipment == null)
            {
                player.LumenSlot02EquipmentID = -1; // -1 WILL NEVER BE AN ITEM ID

                player.playerInventoryManager.lumenSlot2Item = null;
                
                // CALCULATE ALL STAT CHANGES
                LoadLumenSlot();

                return;
            }

            // IF YOU HAVE AN "OnItemEquipped" CALL ON YOUR EQUIPMENT, RUN IT HERE

            // SET CURRENT LUMEN EQUIPMENT IN PLAYER INVENTORY TO THE EQUIPMENT THAT IS PASSED TO THIS FUNCTION
            player.playerInventoryManager.lumenSlot2Item = equipment;

            // CALCULATE ALL STAT CHANGES
            LoadLumenSlot();

            player.LumenSlot02EquipmentID = equipment.itemID;
        }

        public void LoadLumenSlot3Equipment(LumenItem equipment)
        {
            // IF EQUIPMENT IS NULL, SIMPLY SET EQUIPMENT IN INVENTORY TO NULL AND RETURN
            if (equipment == null)
            {
                player.LumenSlot03EquipmentID = -1; // -1 WILL NEVER BE AN ITEM ID

                player.playerInventoryManager.lumenSlot3Item = null;
                
                // CALCULATE ALL STAT CHANGES
                LoadLumenSlot();

                return;
            }

            // IF YOU HAVE AN "OnItemEquipped" CALL ON YOUR EQUIPMENT, RUN IT HERE

            // SET CURRENT LUMEN EQUIPMENT IN PLAYER INVENTORY TO THE EQUIPMENT THAT IS PASSED TO THIS FUNCTION
            player.playerInventoryManager.lumenSlot3Item = equipment;

            // CALCULATE ALL STAT CHANGES
            LoadLumenSlot();

            player.LumenSlot03EquipmentID = equipment.itemID;
        }

        public void LoadLumenSlot4Equipment(LumenItem equipment)
        {            
            // IF EQUIPMENT IS NULL, SIMPLY SET EQUIPMENT IN INVENTORY TO NULL AND RETURN
            if (equipment == null)
            {
                player.LumenSlot04EquipmentID = -1; // -1 WILL NEVER BE AN ITEM ID

                player.playerInventoryManager.lumenSlot4Item = null;
                
                // CALCULATE ALL STAT CHANGES
                LoadLumenSlot();

                return;
            }

            // IF YOU HAVE AN "OnItemEquipped" CALL ON YOUR EQUIPMENT, RUN IT HERE

            // SET CURRENT LUMEN EQUIPMENT IN PLAYER INVENTORY TO THE EQUIPMENT THAT IS PASSED TO THIS FUNCTION
            player.playerInventoryManager.lumenSlot4Item = equipment;

            // CALCULATE ALL STAT CHANGES
            LoadLumenSlot();

            player.LumenSlot04EquipmentID = equipment.itemID;
        }

        public void LoadLumenSlot()
        {            
            // RTSR CHECK
            CheckLumenForRTSR();

            // UPGRADED PARRY CHECK
            CheckLumenForUpgradedParry();

            // CALCULATE OTHER STATS
            player.playerStatsManager.CalculateLumenEquippedStatModifiers();
        }

        private void CheckLumenForRTSR()
        {
            // ENABLE RTSR
            player.rtsrEnabled = false;

            // RTSR CHECK ON SLOT 1
            if (player.playerInventoryManager.lumenSlot1Item != null &&
                player.playerInventoryManager.lumenSlot1Item.increaseDamageAtLowHP)
            {
                player.rtsrEnabled = true;
            }

            // RTSR CHECK ON SLOT 2
            if (player.playerInventoryManager.lumenSlot2Item != null &&
                player.playerInventoryManager.lumenSlot2Item.increaseDamageAtLowHP)
            {
                player.rtsrEnabled = true;
            }

            // RTSR CHECK ON SLOT 3
            if (player.playerInventoryManager.lumenSlot3Item != null &&
                player.playerInventoryManager.lumenSlot3Item.increaseDamageAtLowHP)
            {
                player.rtsrEnabled = true;
            }

            // RTSR CHECK ON SLOT 4
            if (player.playerInventoryManager.lumenSlot4Item != null &&
                player.playerInventoryManager.lumenSlot4Item.increaseDamageAtLowHP)
            {
                player.rtsrEnabled = true;
            }
        }

        private void CheckLumenForUpgradedParry()
        {
            // ENABLE UPGRADED PARRY
            player.upgradedParry = false;

            // UPGRADED PARRY CHECK ON SLOT 1
            if (player.playerInventoryManager.lumenSlot1Item != null &&
                player.playerInventoryManager.lumenSlot1Item.increaseParryWindow)
            {
                player.upgradedParry = true;
            }

            // UPGRADED PARRY CHECK ON SLOT 2
            if (player.playerInventoryManager.lumenSlot2Item != null &&
                player.playerInventoryManager.lumenSlot2Item.increaseParryWindow)
            {
                player.upgradedParry = true;
            }

            // UPGRADED PARRY CHECK ON SLOT 3
            if (player.playerInventoryManager.lumenSlot3Item != null &&
                player.playerInventoryManager.lumenSlot3Item.increaseParryWindow)
            {
                player.upgradedParry = true;
            }

            // UPGRADED PARRY CHECK ON SLOT 4
            if (player.playerInventoryManager.lumenSlot4Item != null &&
                player.playerInventoryManager.lumenSlot4Item.increaseParryWindow)
            {
                player.upgradedParry = true;
            }
        }

        // WEAPON
        private void InitializeWeaponSlot()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponInHand()
        {
            LoadRightWeapon();
        }

        public void SwitchRightWeapon()
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("PlayerCharacter_Equip", false, false, true, true);

            WeaponItem selectedWeapon = null;
            
            player.playerInventoryManager.rightHandWeaponIndex += 1;

            // IF OUR INDEX IS OUT OF BOUNDS, RESET IT TO POSITION #1 (index 0)
            if (player.playerInventoryManager.rightHandWeaponIndex < 0 || 
                player.playerInventoryManager.rightHandWeaponIndex > player.playerInventoryManager.weaponsInRightHandSlots.Length - 1)
            {
                player.playerInventoryManager.rightHandWeaponIndex = 0;

                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;

                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }
                
                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.rightHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.instance.unarmedWeapon;
                    player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                    player.CurrentRightHandWeaponID = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                    player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                    player.CurrentRightHandWeaponID = firstWeapon.itemID;
                }

                return;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
            {
                // IF THE NEXT WEAPON DOES NOT EQUAL THE UNARMED WEAPON, PROCEED
                if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                    player.playerInventoryManager.currentRightHandWeapon = selectedWeapon;
                    player.CurrentRightHandWeaponID = selectedWeapon.itemID;
                    return;
                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= player.playerInventoryManager.weaponsInRightHandSlots.Length - 1)
            {
                SwitchRightWeapon();
            }
        }

        public void LoadRightWeapon()
        {
            if (player.playerInventoryManager.currentRightHandWeapon != null)
            {
                // REMOVE THE OLD WEAPON
                rightHandSlot.UnloadWeapon();

                // BRING IN THE NEW WEAPON
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
                // ASSIGN WEAPON DAMAGE, TO ITS COLLIDER
            }
        }

        // ATTACK COLLISIONS

        public void OpenDamageCollider()
        {
            rightWeaponManager.meleeDamageCollider.EnableCollider();
        }

        public void CloseDamageCollider()
        {
            rightWeaponManager.meleeDamageCollider.DisableCollider();
        }
    
        // UNHIDE WEAPONS
        public void UnHideWeapons()
        {
            if (rightHandWeaponModel != null)
                rightHandWeaponModel.SetActive(true);
        }
    }
}
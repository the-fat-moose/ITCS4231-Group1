using UnityEngine;

namespace Group1 {
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        [Header("Weapon Model Instantiation Slot")]
        public WeaponModelInstantiationSlot rightHandSlot;

        [Header("Weapon Managers")]
        [SerializeField] WeaponManager rightWeaponManager;

        [Header("Weapon Models")]
        public GameObject rightHandWeaponModel;

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

            foreach (QuickSlotItem item in player.playerInventoryManager.quickSlotItemsInQuickSlots)
            {
                // IF THE NEXT WEAPON DOES NOT EQUAL THE UNARMED WEAPON, PROCEED
                if (player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex] != null)
                {
                    selectedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex];
                    player.CurrentQuickSlotItemID = player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex].itemID;
                    return;
                }
            }
        }

        // EQUIPMENT

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

                    player.CurrentRightHandWeaponID = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;

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
                    player.CurrentRightHandWeaponID = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
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
using UnityEngine;

namespace Group1 {
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        public WeaponModelInstantiationSlot rightHandSlot;

        [SerializeField] WeaponManager rightWeaponManager;

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
            //player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false);

            WeaponItem selectedWeapon = null;
            
            player.playerInventoryManager.rightHandWeaponIndex += 1;

            // IF OUR INDEX IS OUT OF BOUNDS, RESET IT TO POSITION #1 (index 0)
            if (player.playerInventoryManager.rightHandWeaponIndex < 0 || 
                player.playerInventoryManager.rightHandWeaponIndex > player.playerInventoryManager.weaponsInRightHandSlots.Length)
            {
                player.playerInventoryManager.rightHandWeaponIndex = 0;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
            {
                // IF THE NEXT WEAPON DOES NOT EQUAL THE UNARMED WEAPON, PROCEED
                if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                    player.CurrentRightHandWeaponID = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                }
            }

            if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex < player.playerInventoryManager.weaponsInRightHandSlots.Length)
            {
                SwitchRightWeapon();
            }
            else
            {
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
                    selectedWeapon = Instantiate(WorldItemDatabase.instance.unarmedWeapon);

                    player.CurrentRightHandWeaponID = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;

                    player.CurrentRightHandWeaponID = firstWeapon.itemID;
                }
            }
        }

        public void LoadRightWeapon()
        {
            if (player.playerInventoryManager.currentRightHandWeapon != null)
            {
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
                // ASSIGN WEAPON DAMAGE, TO ITS COLLIDER
            }
        }
    }
}
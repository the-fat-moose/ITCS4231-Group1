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

        public void LoadRightWeapon()
        {
            if (player.playerInventoryManager.currentRightHandWeapon != null)
            {
                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(player.playerInventoryManager.currentRightHandWeapon);
                // ASSIGN WEAPON DAMAGE, TO ITS COLLIDER
            }
        }
    }
}
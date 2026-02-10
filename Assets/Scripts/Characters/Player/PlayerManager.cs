using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Group1{
    public class PlayerManager : CharacterManager
    {
        [Header("DEBUG MENU")]
        [SerializeField] bool respawnCharacter = false;
        [SerializeField] bool setNewHealth = false;
        [SerializeField] [Range(0, 100)] int newHealthPercentage = 0;
        [SerializeField] bool switchRightWeapon = false;

        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager locomotion;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;

        [Header("Equipment")]
        private int currentRightHandWeaponID = 0;
        private int currentWeaponBeingUsed = 0;

        public event System.Action<int, int> OnRightHandWeaponIDChanged;
        public event System.Action<int, int> OnCurrentWeaponBeingUsedChanged;

        public int CurrentRightHandWeaponID
        {
            get => currentRightHandWeaponID;
            set
            {
                if (currentRightHandWeaponID == value) return;

                int oldValue = currentRightHandWeaponID;
                currentRightHandWeaponID = value;
                OnRightHandWeaponIDChanged?.Invoke(oldValue, currentRightHandWeaponID);
            }
        }

        public int CurrentWeaponBeingUsed
        {
            get => currentWeaponBeingUsed;
            set
            {
                if (currentWeaponBeingUsed == value) return;

                int oldValue = currentWeaponBeingUsed;
                currentWeaponBeingUsed = value;
                OnCurrentWeaponBeingUsedChanged?.Invoke(oldValue, currentWeaponBeingUsed);
            }
        }

        protected override void Awake()
        {
            base.Awake();   //runs CharacterManager Awake

            locomotion = GetComponent<PlayerLocomotionManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();

            // Equipment
            OnRightHandWeaponIDChanged += OnCurrentRightHandWeaponIDChange;
            OnCurrentWeaponBeingUsedChanged += OnCurrentWeaponBeingUsedIDChange;
        }

        protected override void Start()
        {
            base.Start();

            if (PlayerUIManager.instance != null)
            {
                // UPDATE TOTAL AMOUNT OF HEALTH, STAMINA, MANA WHEN THE STAT LINKED TO EITHER CHANGES
                OnEnduranceChanged += SetNewMaxStaminaValue;
                OnVitalityChanged += SetNewMaxHealthValue;
                OnMindChanged += SetNewMaxManaValue;

                // Stamina Setup
                OnStaminaChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                OnStaminaChanged += playerStatsManager.ResetStaminaRegenTimer;
                
                MaxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(Endurance);
                CurrentStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(Endurance);
                PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(MaxStamina);

                // Health Setup
                OnHealthChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;

                MaxHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(Vitality);
                CurrentHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(Vitality);
                PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(MaxHealth);

                // Mana Setup
                OnManaChanged += PlayerUIManager.instance.playerUIHudManager.SetNewManaValue;

                MaxMana = playerStatsManager.CalculateManaBasedOnMindLevel(Mind);
                CurrentMana = playerStatsManager.CalculateManaBasedOnMindLevel(Mind);
                PlayerUIManager.instance.playerUIHudManager.SetMaxManaValue(MaxMana);

                // Death and Healing Handling
                OnHealthChanged += CheckHP;
            }
        }

        protected override void Update()
        {
            base.Update();

            //Handles movement
            locomotion.HandleMovement();

            // REGEN STAMINA
            playerStatsManager.RegenerateStamina();



            // DEBUG DELETE LATER
            DebugMenu();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            PlayerCamera.cam.HandleCameraActions();
        }

        public override IEnumerator ProcessDeathEvent()
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();

            return base.ProcessDeathEvent();

            // CHECK FOR PLAYERS THAT ARE ALIVE, IF 0 RESPAWN CHARACTERS
        }

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();

            CurrentHealth = MaxHealth;
            CurrentStamina = MaxStamina;
            CurrentMana = MaxMana;

            isDead = false;

            // PLAY REBIRTH EFFECTS
            // playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
        }

        private void SetNewMaxHealthValue(int oldVitality, int newVitality)
        {
            MaxHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(MaxHealth);
            CurrentHealth = MaxHealth;
        }

        private void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            MaxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(MaxStamina);
            CurrentStamina = MaxStamina;
        }

        private void SetNewMaxManaValue(int oldMind, int newMind)
        {
            MaxMana = playerStatsManager.CalculateManaBasedOnMindLevel(newMind);
            PlayerUIManager.instance.playerUIHudManager.SetMaxManaValue(MaxMana);
            CurrentMana = MaxMana;
        }

        public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
            playerInventoryManager.currentRightHandWeapon = newWeapon;
            playerEquipmentManager.LoadRightWeapon();
        }

        public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
            playerCombatManager.currentWeaponBeingUsed = newWeapon;
        }

        public void PerformWeaponBasedAction(int actionID, int weaponID)
        {
            WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);

            if (weaponAction != null)
            {
                weaponAction.AttemptToPerformAction(this, WorldItemDatabase.instance.GetWeaponByID(weaponID));
            }
            else
            {
                Debug.LogError("ACTION IS NULL, CANNOT BE PERFORMED");
            }
        }

        // DEBUG DELETE LATER
        private void DebugMenu()
        {
            if (respawnCharacter)
            {
                respawnCharacter = false;
                ReviveCharacter();
            }

            if (setNewHealth)
            {
                setNewHealth = false;
                CurrentHealth = MaxHealth * newHealthPercentage / 100;
            }

            if (switchRightWeapon)
            {
                switchRightWeapon = false;
                playerEquipmentManager.SwitchRightWeapon();
            }
        }

        public void DebugRespawnPlayer()
        {
            respawnCharacter = true;
            GameObject playerUIManager = GameObject.Find("PlayerUIManager");
            if (playerUIManager != null)
            {
                // playerUIManager/Hud Manager/Debug Manager/Debug Panel/Respawn Toggle
                playerUIManager.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Toggle>().isOn = false;
            }
        }

        public void DebugDie()
        {
            setNewHealth = true;
            GameObject playerUIManager = GameObject.Find("PlayerUIManager");
            if (playerUIManager != null)
            {
                // playerUIManager/Hud Manager/Debug Manager/Debug Panel/Die Toggle
                playerUIManager.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).GetComponent<Toggle>().isOn = false;
            }
        }

        public void DebugSwitchWeapon()
        {
            switchRightWeapon = true;
            GameObject playerUIManager = GameObject.Find("PlayerUIManager");
            if (playerUIManager != null)
            {
                // playerUIManager/Hud Manager/Debug Manager/Debug Panel/Switch Weapon Toggle
                playerUIManager.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).GetComponent<Toggle>().isOn = false;
            }
        }
    }       
}

using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Group1{
    public class PlayerManager : CharacterManager
    {
        [Header("Generic Save Data Variables")]
        public FixedString64Bytes characterName = "Character";

        [Header("DEBUG MENU")]
        [SerializeField] bool respawnCharacter = false;
        [SerializeField] bool setNewHealth = false;
        [SerializeField] [Range(0, 100)] int newHealthPercentage = 0;

        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager locomotion;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        [HideInInspector] public PlayerInteractionManager playerInteractionManager;
        [HideInInspector] public PlayerEffectsManager playerEffectsManager;

        #region Equipment Variables

        [Header("Lumen Stats")]
        public bool rtsrEnabled = false;
        [SerializeField] float lowHealthPercentage = 0.2f;
        public bool isAtLowHealth = false;
        [SerializeField] GameObject lowHealthParticles;
        public float baseFlaskRestorationMultiplier = 1f;
        public float flaskRestorationMultiplier = 1f;

        [Header("Equipment")]
        private int currentRightHandWeaponID = 0;
        private int currentWeaponBeingUsed = 0;
        private int currentQuickSlotItemID = 0;
        private int lumenSlot01EquipmentID = -1;
        private int lumenSlot02EquipmentID = -1;
        private int lumenSlot03EquipmentID = -1;
        private int lumenSlot04EquipmentID = -1;

        public event System.Action<int, int> OnRightHandWeaponIDChanged;
        public event System.Action<int, int> OnCurrentWeaponBeingUsedChanged;
        public event System.Action<int, int> OnCurrentQuickSlotItemChanged;
        public event System.Action<int, int> OnLumenSlot01EquipmentIDChanged;
        public event System.Action<int, int> OnLumenSlot02EquipmentIDChanged;
        public event System.Action<int, int> OnLumenSlot03EquipmentIDChanged;
        public event System.Action<int, int> OnLumenSlot04EquipmentIDChanged;

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

        public int CurrentQuickSlotItemID
        {
            get => currentQuickSlotItemID;
            set
            {
                if (currentQuickSlotItemID == value) return;

                int oldValue = currentQuickSlotItemID;
                currentQuickSlotItemID = value;
                OnCurrentQuickSlotItemChanged?.Invoke(oldValue, currentQuickSlotItemID);
            }
        }

        public int LumenSlot01EquipmentID
        {
            get => lumenSlot01EquipmentID;
            set
            {
                if (lumenSlot01EquipmentID == value) return;

                int oldValue = lumenSlot01EquipmentID;
                lumenSlot01EquipmentID = value;
                OnLumenSlot01EquipmentIDChanged?.Invoke(oldValue, lumenSlot01EquipmentID);
            }
        }

        public int LumenSlot02EquipmentID
        {
            get => lumenSlot02EquipmentID;
            set
            {
                if (lumenSlot02EquipmentID == value) return;

                int oldValue = lumenSlot02EquipmentID;
                lumenSlot02EquipmentID = value;
                OnLumenSlot02EquipmentIDChanged?.Invoke(oldValue, lumenSlot02EquipmentID);
            }
        }

        public int LumenSlot03EquipmentID
        {
            get => lumenSlot03EquipmentID;
            set
            {
                if (lumenSlot03EquipmentID == value) return;

                int oldValue = lumenSlot03EquipmentID;
                lumenSlot03EquipmentID = value;
                OnLumenSlot03EquipmentIDChanged?.Invoke(oldValue, lumenSlot03EquipmentID);
            }
        }

        public int LumenSlot04EquipmentID
        {
            get => lumenSlot04EquipmentID;
            set
            {
                if (lumenSlot04EquipmentID == value) return;

                int oldValue = lumenSlot04EquipmentID;
                lumenSlot04EquipmentID = value;
                OnLumenSlot04EquipmentIDChanged?.Invoke(oldValue, lumenSlot04EquipmentID);
            }
        }

        #endregion

        [Header("Flasks")]
        public int remainingHealthFlasks = 0;
        private bool isChugging = false;

        public event System.Action<bool, bool> OnIsChuggingChanged;

        public bool IsChugging
        {
            get => isChugging;
            set
            {
                if (isChugging == value) return;

                bool oldValue = isChugging;
                isChugging = value;
                OnIsChuggingChanged?.Invoke(oldValue, isChugging);
            }
        }

        #region Unity Functions

        protected override void Awake()
        {
            base.Awake();   //runs CharacterManager Awake

            locomotion = GetComponent<PlayerLocomotionManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInteractionManager = GetComponent<PlayerInteractionManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }

        protected override void Start()
        {
            base.Start();

            PlayerCamera.cam.player = this;
            PlayerInputManager.inputs.player = this;
            WorldSaveGameManager.instance.player = this;

            if (PlayerUIManager.instance != null)
            {
                // UPDATE TOTAL AMOUNT OF HEALTH, STAMINA, MANA WHEN THE STAT LINKED TO EITHER CHANGES
                OnEnduranceChanged += SetNewMaxStaminaValue;
                OnVitalityChanged += SetNewMaxHealthValue;
                OnMindChanged += SetNewMaxManaValue;

                // Stamina Setup
                OnStaminaChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                OnStaminaChanged += playerStatsManager.ResetStaminaRegenTimer;

                // Health Setup
                OnHealthChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;

                // Mana Setup
                OnManaChanged += PlayerUIManager.instance.playerUIHudManager.SetNewManaValue;

                // Death and Healing Handling
                OnHealthChanged += CheckHP;

                // Equipment
                OnRightHandWeaponIDChanged += OnCurrentRightHandWeaponIDChange;
                OnCurrentWeaponBeingUsedChanged += OnCurrentWeaponBeingUsedIDChange;
                OnCurrentQuickSlotItemChanged += OnCurrentQuickSlotItemIDChange;
                OnIsChuggingChanged += OnIsChuggingValueChanged;

                OnLumenSlot01EquipmentIDChanged += OnLumenSlot01EquipmentChanged;
                OnLumenSlot02EquipmentIDChanged += OnLumenSlot02EquipmentChanged;
                OnLumenSlot03EquipmentIDChanged += OnLumenSlot03EquipmentChanged;
                OnLumenSlot04EquipmentIDChanged += OnLumenSlot04EquipmentChanged;
            }
        }

        protected override void Update()
        {
            base.Update();

            //Handles movement
            locomotion.HandleMovement();

            // REGEN STAMINA
            playerStatsManager.RegenerateStamina();

            if (setNewHealth)
            {
                setNewHealth = false;
                CurrentHealth = newHealthPercentage * MaxHealth;
            }

            DebugMenu();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            PlayerCamera.cam.HandleCameraActions();
        }

        #endregion

        #region Player Saving and Loading

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentCharacterData.sceneIndex <= 0)
            {
                currentCharacterData.sceneIndex = 1; // DEFAULT FIRST PLAYABLE SCENE
            }
            
            currentCharacterData.characterName = characterName.ToString();
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;

            currentCharacterData.currentStamina = CurrentStamina;
            currentCharacterData.currentHealth = CurrentHealth;
            currentCharacterData.currentMana = CurrentMana;

            currentCharacterData.endurance = Endurance;
            currentCharacterData.vitality = Vitality;
            currentCharacterData.mind = Mind;

            // EQUIPMENT
            currentCharacterData.rightWeaponIndex = playerInventoryManager.rightHandWeaponIndex;

            currentCharacterData.rightWeapon01 = playerInventoryManager.weaponsInRightHandSlots[0].itemID; // THIS SHOULD NEVER BE NULL (should always default to unarmed)
            currentCharacterData.rightWeapon02 = playerInventoryManager.weaponsInRightHandSlots[1].itemID; // THIS SHOULD NEVER BE NULL (should always default to unarmed)
            currentCharacterData.rightWeapon03 = playerInventoryManager.weaponsInRightHandSlots[2].itemID; // THIS SHOULD NEVER BE NULL (should always default to unarmed)

            // LUMENS
            currentCharacterData.lumen01 = LumenSlot01EquipmentID;
            currentCharacterData.lumen02 = LumenSlot02EquipmentID;
            currentCharacterData.lumen03 = LumenSlot03EquipmentID;
            currentCharacterData.lumen04 = LumenSlot04EquipmentID;
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            // ------------ NAME ------------
            characterName = currentCharacterData.characterName;
            
            // ------------ POSITION ------------
            Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = myPosition;

            // ------------ STATS ------------
            Endurance = currentCharacterData.endurance;
            Vitality = currentCharacterData.vitality;
            Mind = currentCharacterData.mind;

            // ------------ RESOURCES ------------
            MaxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(Endurance);
            CurrentStamina = MaxStamina;
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(MaxStamina);

            MaxHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(Vitality);
            if (currentCharacterData.currentHealth <= 0)
            {
                CurrentHealth = MaxHealth;
            }
            else
            {
                CurrentHealth = currentCharacterData.currentHealth;
            }
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(MaxHealth);

            MaxMana = playerStatsManager.CalculateManaBasedOnMindLevel(Mind);
            if (currentCharacterData.currentMana <= 0)
            {
                CurrentMana = MaxMana;
            }
            else
            {
                CurrentMana = currentCharacterData.currentMana;
            }
            PlayerUIManager.instance.playerUIHudManager.SetMaxManaValue(MaxMana);

            // ------------ EQUIPMENT ------------
            // WEAPON EQUIPMENT 
            if (WorldItemDatabase.instance.GetWeaponByID(currentCharacterData.rightWeapon01))
            {
                WeaponItem rightWeapon01 = Instantiate(WorldItemDatabase.instance.GetWeaponByID(currentCharacterData.rightWeapon01));
                playerInventoryManager.weaponsInRightHandSlots[0] = rightWeapon01;
            }
            else
            {
                playerInventoryManager.weaponsInRightHandSlots[0] = null;
            }

            if (WorldItemDatabase.instance.GetWeaponByID(currentCharacterData.rightWeapon02))
            {
                WeaponItem rightWeapon02 = Instantiate(WorldItemDatabase.instance.GetWeaponByID(currentCharacterData.rightWeapon02));
                playerInventoryManager.weaponsInRightHandSlots[1] = rightWeapon02;
            }
            else
            {
                playerInventoryManager.weaponsInRightHandSlots[1] = null;
            }

            if (WorldItemDatabase.instance.GetWeaponByID(currentCharacterData.rightWeapon03))
            {
                WeaponItem rightWeapon03 = Instantiate(WorldItemDatabase.instance.GetWeaponByID(currentCharacterData.rightWeapon03));
                playerInventoryManager.weaponsInRightHandSlots[2] = rightWeapon03;
            }
            else
            {
                playerInventoryManager.weaponsInRightHandSlots[2] = null;
            }

            playerInventoryManager.rightHandWeaponIndex = currentCharacterData.rightWeaponIndex;
            CurrentRightHandWeaponID = playerInventoryManager.weaponsInRightHandSlots[currentCharacterData.rightWeaponIndex].itemID;

            // LUMEN EQUIPMENT
            if (WorldItemDatabase.instance.GetLumenItemByID(currentCharacterData.lumen01))
            {
                LumenItem lumenSlot01Equipment = Instantiate(WorldItemDatabase.instance.GetLumenItemByID(currentCharacterData.lumen01));
                playerInventoryManager.lumenSlot1Item = lumenSlot01Equipment;
            }
            else
            {
                playerInventoryManager.lumenSlot1Item = null;
            }
            
            if (WorldItemDatabase.instance.GetLumenItemByID(currentCharacterData.lumen02))
            {
                LumenItem lumenSlot02Equipment = Instantiate(WorldItemDatabase.instance.GetLumenItemByID(currentCharacterData.lumen02));
                playerInventoryManager.lumenSlot2Item = lumenSlot02Equipment;
            }
            else
            {
                playerInventoryManager.lumenSlot2Item = null;
            }
            
            if (WorldItemDatabase.instance.GetLumenItemByID(currentCharacterData.lumen03))
            {
                LumenItem lumenSlot03Equipment = Instantiate(WorldItemDatabase.instance.GetLumenItemByID(currentCharacterData.lumen03));
                playerInventoryManager.lumenSlot3Item = lumenSlot03Equipment;
            }
            else
            {
                playerInventoryManager.lumenSlot3Item = null;
            }

            if (WorldItemDatabase.instance.GetLumenItemByID(currentCharacterData.lumen04))
            {
                LumenItem lumenSlot04Equipment = Instantiate(WorldItemDatabase.instance.GetLumenItemByID(currentCharacterData.lumen04));
                playerInventoryManager.lumenSlot4Item = lumenSlot04Equipment;
            }
            else
            {
                playerInventoryManager.lumenSlot4Item = null;
            }

            // PLAYER ACTIVATION
            canMove = true;
            canRun = true;
            canRotate = true;
        }

        #endregion

        #region Health and Death Handling

        public override void CheckHP(int oldValue, int newValue)
        {
            if (CurrentHealth <= 0)
            {
                StartCoroutine(ProcessDeathEvent());
            }

            // ENABLE EXTRA DAMAGE BOOL
            if (CurrentHealth < (MaxHealth * lowHealthPercentage) && rtsrEnabled)
            {
                isAtLowHealth = true;
                lowHealthParticles.SetActive(true);

                if (playerInventoryManager.currentRightHandWeapon != null)
                {
                    playerEquipmentManager.rightWeaponManager.SetWeaponDamage(this, playerInventoryManager.currentRightHandWeapon, true);
                }
            }
            else
            {
                isAtLowHealth = false;
                lowHealthParticles.SetActive(false);

                if (playerInventoryManager.currentRightHandWeapon != null)
                {
                    playerEquipmentManager.rightWeaponManager.SetWeaponDamage(this, playerInventoryManager.currentRightHandWeapon, false);
                }
            }

            // PREVENTS US FROM OVER HEALING
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
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
            playerAnimatorManager.PlayTargetActionAnimation("Empty", false, true, false, false);
        }

        #endregion

        #region Stat Setting

        public override void SetNewMaxHealthValue(int oldVitality, int newVitality)
        {
            MaxHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(MaxHealth);
            CurrentHealth = MaxHealth;
        }

        public override void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
        {
            MaxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(MaxStamina);
            CurrentStamina = MaxStamina;
        }

        public override void SetNewMaxManaValue(int oldMind, int newMind)
        {
            MaxMana = playerStatsManager.CalculateManaBasedOnMindLevel(newMind);
            PlayerUIManager.instance.playerUIHudManager.SetMaxManaValue(MaxMana);
            CurrentMana = MaxMana;
        }

        // Lumen Item Equipping
        public void RecalibrateStatValues()
        {
            MaxHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(Vitality);
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(MaxHealth);
            PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue(CurrentHealth, CurrentHealth);
            
            MaxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(Endurance);
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(MaxStamina);
            PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue(CurrentStamina, CurrentStamina);

            MaxMana = playerStatsManager.CalculateManaBasedOnMindLevel(Mind);
            PlayerUIManager.instance.playerUIHudManager.SetMaxManaValue(MaxMana);
            PlayerUIManager.instance.playerUIHudManager.SetNewManaValue(CurrentMana, CurrentMana);
        }

        #endregion

        #region Actions

        public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
            playerInventoryManager.currentRightHandWeapon = newWeapon;
            playerEquipmentManager.LoadRightWeapon();

            PlayerUIManager.instance.playerUIHudManager.SetWeaponQuickSlotIcon(newID);

            // RTSR CHECK
            if (playerInventoryManager.currentRightHandWeapon != null && rtsrEnabled && isAtLowHealth)
            {
                playerEquipmentManager.rightWeaponManager.SetWeaponDamage(this, playerInventoryManager.currentRightHandWeapon, true);
            }
            else
            {
                playerEquipmentManager.rightWeaponManager.SetWeaponDamage(this, playerInventoryManager.currentRightHandWeapon, false);
            }
        }

        public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
            playerCombatManager.currentWeaponBeingUsed = newWeapon;
        }

        public void OnCurrentQuickSlotItemIDChange(int oldID, int newID)
        {
            QuickSlotItem newQuickSlotItem = null;

            if (WorldItemDatabase.instance.GetQuickSlotItemByID(newID))
                newQuickSlotItem = Instantiate(WorldItemDatabase.instance.GetQuickSlotItemByID(newID));
            
            if (newQuickSlotItem != null)
            {
                playerInventoryManager.currentQuickSlotItem = newQuickSlotItem;
            }
            else
            {
                playerInventoryManager.currentQuickSlotItem = null;
            }                

            PlayerUIManager.instance.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(newID);
        }

        public void OnIsChuggingValueChanged(bool oldStatus, bool newStatus)
        {
            animator.SetBool("isChuggingFlask", IsChugging);
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

        public void PerformAbilityAction(int abilityID)
        {
            PlayerAbilityAction abilityAction = WorldActionManager.instance.GetPlayerAbilityActionByID(abilityID);

            if (abilityAction != null)
            {
                abilityAction.AttemptToPerformAbility(this);
            }
            else
            {
                Debug.LogError("ABILITY ACTION IS NULL, CANNOT BE PERFORMED");
            }
        }

        public void HideWeapons()
        {
            if (playerEquipmentManager.rightHandWeaponModel != null)
                playerEquipmentManager.rightHandWeaponModel.SetActive(false);
        }

        // LUMEN EQUIPMENT
        public void OnLumenSlot01EquipmentChanged(int oldValue, int newValue)
        {
            LumenItem equipment = WorldItemDatabase.instance.GetLumenItemByID(LumenSlot01EquipmentID);

            if (equipment != null)
            {
                playerEquipmentManager.LoadLumenSlot1Equipment(Instantiate(equipment));
            }
            else
            {
                playerEquipmentManager.LoadLumenSlot1Equipment(null);
            }
        }

        public void OnLumenSlot02EquipmentChanged(int oldValue, int newValue)
        {
            LumenItem equipment = WorldItemDatabase.instance.GetLumenItemByID(LumenSlot02EquipmentID);

            if (equipment != null)
            {
                playerEquipmentManager.LoadLumenSlot2Equipment(Instantiate(equipment));
            }
            else
            {
                playerEquipmentManager.LoadLumenSlot2Equipment(null);
            }
        }

        public void OnLumenSlot03EquipmentChanged(int oldValue, int newValue)
        {
            LumenItem equipment = WorldItemDatabase.instance.GetLumenItemByID(LumenSlot03EquipmentID);

            if (equipment != null)
            {
                playerEquipmentManager.LoadLumenSlot3Equipment(Instantiate(equipment));
            }
            else
            {
                playerEquipmentManager.LoadLumenSlot3Equipment(null);
            }
        }

        public void OnLumenSlot04EquipmentChanged(int oldValue, int newValue)
        {
            LumenItem equipment = WorldItemDatabase.instance.GetLumenItemByID(LumenSlot04EquipmentID);

            if (equipment != null)
            {
                playerEquipmentManager.LoadLumenSlot4Equipment(Instantiate(equipment));
            }
            else
            {
                playerEquipmentManager.LoadLumenSlot4Equipment(null);
            }
        }

        #endregion

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
        }
    }       
}

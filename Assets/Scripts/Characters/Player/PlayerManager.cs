using System.Collections;
using System.Collections.Generic;
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
        public int maxHealthFlasks {get; private set;} = 5;
        private int remainingHealthFlasks = 0;
        private bool isChugging = false;

        public event System.Action<bool, bool> OnIsChuggingChanged;
        public event System.Action<int, int> OnRemainingHealthFlasksValueChanged;

        public int RemainingHealthFlasks
        {
            get => remainingHealthFlasks;
            set
            {
                if (remainingHealthFlasks == value) return;

                int oldValue = remainingHealthFlasks;
                remainingHealthFlasks = value;
                OnRemainingHealthFlasksValueChanged?.Invoke(oldValue, remainingHealthFlasks);
            }
        }

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

            DontDestroyOnLoad(this);

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

                OnRemainingHealthFlasksValueChanged += OnRemainingHealthFlasksChanged;

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
            // --------------- WORLD SCENE ---------------
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentCharacterData.sceneIndex <= 0)
            {
                currentCharacterData.sceneIndex = 1; // DEFAULT FIRST PLAYABLE SCENE
            }
            
            // --------------- NAME ---------------
            currentCharacterData.characterName = characterName.ToString();

            // --------------- POSITION ---------------
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;

            // --------------- RESOURCES ---------------
            currentCharacterData.currentStamina = CurrentStamina;
            currentCharacterData.currentHealth = CurrentHealth;
            currentCharacterData.currentMana = CurrentMana;

            // --------------- STATS ---------------
            currentCharacterData.endurance = Endurance;
            currentCharacterData.vitality = Vitality;
            currentCharacterData.mind = Mind;

            // --------------- EQUIPMENT ---------------
            // WEAPONS
            currentCharacterData.rightWeaponIndex = playerInventoryManager.rightHandWeaponIndex;

            currentCharacterData.rightWeapon01 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlots[0]); // THIS SHOULD NEVER BE NULL (should always default to unarmed)
            currentCharacterData.rightWeapon02 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlots[1]); // THIS SHOULD NEVER BE NULL (should always default to unarmed)
            currentCharacterData.rightWeapon03 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlots[2]); // THIS SHOULD NEVER BE NULL (should always default to unarmed)

            // QUICK SLOT ITEMS
            currentCharacterData.currentHealthFlasksRemaining = RemainingHealthFlasks;

            currentCharacterData.quickSlotItemIndex = playerInventoryManager.quickSlotItemIndex;
            currentCharacterData.quickSlotItem01 = WorldSaveGameManager.instance.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[0]);
            currentCharacterData.quickSlotItem02 = WorldSaveGameManager.instance.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[1]);
            currentCharacterData.quickSlotItem03 = WorldSaveGameManager.instance.GetSerializableQuickSlotItemFromQuickSlotItem(playerInventoryManager.quickSlotItemsInQuickSlots[2]);

            // LUMENS
            currentCharacterData.lumen01 = LumenSlot01EquipmentID;
            currentCharacterData.lumen02 = LumenSlot02EquipmentID;
            currentCharacterData.lumen03 = LumenSlot03EquipmentID;
            currentCharacterData.lumen04 = LumenSlot04EquipmentID;

            // --------------- INVENTORY ---------------
            
            currentCharacterData.weaponsInInventory.Clear();
            currentCharacterData.lumenEquipmentInInventory.Clear();
            currentCharacterData.quickSlotItemsInInventory.Clear();

            for (int i = 0; i < playerInventoryManager.itemsInInventory.Count; i++)
            {
                if (playerInventoryManager.itemsInInventory[i] == null)
                    continue;
                
                WeaponItem weaponInInventory = playerInventoryManager.itemsInInventory[i] as WeaponItem;
                LumenItem lumenEquipmentInInventory = playerInventoryManager.itemsInInventory[i] as LumenItem;
                QuickSlotItem quickSlotItemInInventory = playerInventoryManager.itemsInInventory[i] as QuickSlotItem;

                if (weaponInInventory != null) currentCharacterData.weaponsInInventory.Add(WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(weaponInInventory));

                if (lumenEquipmentInInventory != null) currentCharacterData.lumenEquipmentInInventory.Add(lumenEquipmentInInventory.itemID);

                if (quickSlotItemInInventory != null) currentCharacterData.quickSlotItemsInInventory.Add(WorldSaveGameManager.instance.GetSerializableQuickSlotItemFromQuickSlotItem(quickSlotItemInInventory));
            }
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
            playerInventoryManager.rightHandWeaponIndex = currentCharacterData.rightWeaponIndex;
            playerInventoryManager.weaponsInRightHandSlots[0] = currentCharacterData.rightWeapon01.GetWeapon();
            playerInventoryManager.weaponsInRightHandSlots[1] = currentCharacterData.rightWeapon02.GetWeapon();
            playerInventoryManager.weaponsInRightHandSlots[2] = currentCharacterData.rightWeapon03.GetWeapon();

            if (currentCharacterData.rightWeaponIndex >= 0) 
            {
                playerInventoryManager.currentRightHandWeapon = playerInventoryManager.weaponsInRightHandSlots[currentCharacterData.rightWeaponIndex];
                CurrentRightHandWeaponID = playerInventoryManager.weaponsInRightHandSlots[currentCharacterData.rightWeaponIndex].itemID;
            }
            else
            {
                CurrentRightHandWeaponID = WorldItemDatabase.instance.unarmedWeapon.itemID;
            }
            
            // QUICK SLOT ITEMS
            RemainingHealthFlasks = currentCharacterData.currentHealthFlasksRemaining;

            playerInventoryManager.quickSlotItemIndex = currentCharacterData.quickSlotItemIndex;
            playerInventoryManager.quickSlotItemsInQuickSlots[0] = currentCharacterData.quickSlotItem01.GetQuickSlotItem();
            playerInventoryManager.quickSlotItemsInQuickSlots[1] = currentCharacterData.quickSlotItem02.GetQuickSlotItem();
            playerInventoryManager.quickSlotItemsInQuickSlots[2] = currentCharacterData.quickSlotItem03.GetQuickSlotItem();
            playerEquipmentManager.LoadQuickSlotEquipment(playerInventoryManager.quickSlotItemsInQuickSlots[playerInventoryManager.quickSlotItemIndex]); // REFRESHES THE HUD

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

            // ------------ INVENTORY ------------
            // WEAPONS
            for (int i = 0; i < currentCharacterData.weaponsInInventory.Count; i++)
            {
                WeaponItem weapon = currentCharacterData.weaponsInInventory[i].GetWeapon();
                playerInventoryManager.AddItemToInventory(weapon);
            }

            // LUMENS
            for (int i = 0; i < currentCharacterData.lumenEquipmentInInventory.Count; i++)
            {
                LumenItem equipment = WorldItemDatabase.instance.GetLumenItemByID(currentCharacterData.lumenEquipmentInInventory[i]);
                playerInventoryManager.AddItemToInventory(equipment);
            }

            // QUICK SLOT ITEMS
            for (int i = 0; i < currentCharacterData.quickSlotItemsInInventory.Count; i++)
            {
                QuickSlotItem item = currentCharacterData.quickSlotItemsInInventory[i].GetQuickSlotItem();
                playerInventoryManager.AddItemToInventory(item);
            }

            // ------------ PLAYER ACTIVATION ------------
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

            // CHECK FOR PLAYERS THAT ARE ALIVE, IF 0 RESPAWN CHARACTERS
            WorldGameSessionManager.instance.WaitThenRevivePlayer();

            return base.ProcessDeathEvent();
        }    

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();

            CurrentHealth = MaxHealth;
            CurrentStamina = MaxStamina;
            CurrentMana = MaxMana;
            RemainingHealthFlasks = maxHealthFlasks;

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

            if (CurrentHealth <= MaxHealth)
                PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue(CurrentHealth, CurrentHealth);
            else
            {
                CurrentHealth = MaxHealth;
                PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue(CurrentHealth, MaxHealth);
            }
            
            MaxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(Endurance);
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(MaxStamina);
            if (CurrentStamina <= MaxStamina)
                PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue(CurrentStamina, CurrentStamina);
            else
            {
                CurrentStamina = MaxStamina;
                PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue(CurrentStamina, MaxStamina);
            }

            MaxMana = playerStatsManager.CalculateManaBasedOnMindLevel(Mind);
            PlayerUIManager.instance.playerUIHudManager.SetMaxManaValue(MaxMana);
            if (CurrentMana <= MaxMana)
                PlayerUIManager.instance.playerUIHudManager.SetNewManaValue(CurrentMana, CurrentMana);
            else
            {
                CurrentMana = MaxMana;
                PlayerUIManager.instance.playerUIHudManager.SetNewManaValue(CurrentMana, CurrentMana);
            }
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

            PlayerUIManager.instance.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(playerInventoryManager.currentQuickSlotItem);
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

        // QUICK SLOT ITEMS
        public void OnRemainingHealthFlasksChanged(int oldValue, int newValue)
        {
            if (WorldItemDatabase.instance.GetQuickSlotItemByID(CurrentQuickSlotItemID) != null)
            {
                PlayerUIManager.instance.playerUIHudManager.quickSlotItemCount.text = WorldItemDatabase.instance.GetQuickSlotItemByID(CurrentQuickSlotItemID).GetCurrentAmount(this).ToString();
            }     
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
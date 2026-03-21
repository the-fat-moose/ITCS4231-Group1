using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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

        [Header("Equipment")]
        private int currentRightHandWeaponID = 0;
        private int currentWeaponBeingUsed = 0;
        public List<int> lumenEquipmentIDs = new List<int>();

        public event System.Action<int, int> OnRightHandWeaponIDChanged;
        public event System.Action<int, int> OnCurrentWeaponBeingUsedChanged;
        public event System.Action<int, int, int> OnLumenEquipmentIDChanged;

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
                OnLumenEquipmentIDChanged += OnLumenEquipmentIDChange;
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

            /*
            currentCharacterData.lumen01 = playerInventoryManager.lumenEquipmentItemSlots[0].itemID;
            currentCharacterData.lumen02 = playerInventoryManager.lumenEquipmentItemSlots[1].itemID;
            currentCharacterData.lumen03 = playerInventoryManager.lumenEquipmentItemSlots[2].itemID;
            currentCharacterData.lumen04 = playerInventoryManager.lumenEquipmentItemSlots[3].itemID;
            */
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
            /*
            if (WorldItemDatabase.instance.GetLumenEquipmentByID(currentCharacterData.lumen01))
            {
                LumenEquipmentItem lumen01 = Instantiate(WorldItemDatabase.instance.GetLumenEquipmentByID(currentCharacterData.lumen01));
                playerInventoryManager.lumenEquipmentItemSlots[0] = lumen01;
            }
            else
            {
                playerInventoryManager.lumenEquipmentItemSlots[0] = null;
            }

            if (WorldItemDatabase.instance.GetLumenEquipmentByID(currentCharacterData.lumen02))
            {
                LumenEquipmentItem lumen02 = Instantiate(WorldItemDatabase.instance.GetLumenEquipmentByID(currentCharacterData.lumen02));
                playerInventoryManager.lumenEquipmentItemSlots[1] = lumen02;
            }
            else
            {
                playerInventoryManager.lumenEquipmentItemSlots[1] = null;
            }

            if (WorldItemDatabase.instance.GetLumenEquipmentByID(currentCharacterData.lumen03))
            {
                LumenEquipmentItem lumen03 = Instantiate(WorldItemDatabase.instance.GetLumenEquipmentByID(currentCharacterData.lumen03));
                playerInventoryManager.lumenEquipmentItemSlots[2] = lumen03;
            }
            else
            {
                playerInventoryManager.lumenEquipmentItemSlots[2] = null;
            }
            
            if (WorldItemDatabase.instance.GetLumenEquipmentByID(currentCharacterData.lumen04))
            {
                LumenEquipmentItem lumen04 = Instantiate(WorldItemDatabase.instance.GetLumenEquipmentByID(currentCharacterData.lumen04));
                playerInventoryManager.lumenEquipmentItemSlots[3] = lumen04;
            }
            else
            {
                playerInventoryManager.lumenEquipmentItemSlots[3] = null;
            }
            */
        }

        #endregion

        #region Death Handling

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

        #endregion

        #region Actions

        public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
            playerInventoryManager.currentRightHandWeapon = newWeapon;
            playerEquipmentManager.LoadRightWeapon();

            PlayerUIManager.instance.playerUIHudManager.SetWeaponQuickSlotIcon(newID);
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

        public void SetLumenEquipmentID(int index, int newID)
        {
            if (index < 0 || index >= lumenEquipmentIDs.Count) return;

            int oldID = lumenEquipmentIDs[index];

            if (oldID == newID) return;

            lumenEquipmentIDs[index] = newID;

            OnLumenEquipmentIDChanged?.Invoke(index, oldID, newID);
        }

        public void OnLumenEquipmentIDChange(int index, int oldID, int newID)
        {
            LumenEquipmentItem lumenEquipmentItem = WorldItemDatabase.instance.GetLumenEquipmentByID(newID);

            if (lumenEquipmentItem != null)
            {
                playerEquipmentManager.LoadLumenEquipment(Instantiate(lumenEquipmentItem), index);
            }
            else
            {
                playerEquipmentManager.LoadLumenEquipment(null, index);
            }
        }

        #endregion

        void OnDrawGizmos()
        {
            if (playerInventoryManager.cageAbility is CageAbility cage)
            {
                cage.DrawDebug(this);
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
        }
    }       
}

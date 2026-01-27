using UnityEngine;

namespace Group1{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager locomotion;
        [HideInInspector] PlayerStatsManager playerStatsManager;

        protected override void Awake()
        {
            base.Awake();   //runs CharacterManager Awake 

            locomotion = GetComponent<PlayerLocomotionManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();

            // UPDATE TOTAL AMOUNT OF HEALTH, STAMINA, MANA WHEN THE STAT LINKED TO EITHER CHANGES
            OnEnduranceChanged += SetNewMaxStaminaValue;
            OnVitalityChanged += SetNewMaxHealthValue;
            OnManaChanged += SetNewMaxManaValue;

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
        }

        protected override void Update()
        {
            base.Update();

            //Handles movement
            locomotion.HandleMovement();

            // REGEN STAMINA
            playerStatsManager.RegenerateStamina();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            PlayerCamera.cam.HandleCameraActions();
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
            PlayerUIManager.instance.playerUIHudManager.SetMaxManaValue(MaxStamina);
            CurrentMana = MaxMana;
        }
    }       
}

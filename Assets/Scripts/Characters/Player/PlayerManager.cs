using UnityEngine;

namespace Group1{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] PlayerLocomotionManager locomotion;
        [HideInInspector] PlayerStatsManager playerStatsManager;

        protected override void Awake()
        {
            base.Awake();   //runs CharacterManager Awake 

            locomotion = GetComponent<PlayerLocomotionManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();

            // UPDATE TOTAL AMOUNT OF HEALTH OR STAMINA WHEN THE STAT LINKED TO EITHER CHANGES
            OnEnduranceChanged += SetNewMaxStaminaValue;
            OnVitalityChanged += SetNewMaxHealthValue;

            // Stamina Setup
            OnStaminaChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
            /* ADD CODE TO RESET STAMINA REGEN TIMER */
            
            MaxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(Endurance);
            CurrentStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(Endurance);
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(MaxStamina);

            // Health Setup
            OnHealthChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;

            MaxHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(Vitality);
            CurrentHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(Vitality);
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(MaxHealth);
        }

        protected override void Update()
        {
            base.Update();

            //Handles movement
            locomotion.HandleMovement();
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
    }       
}

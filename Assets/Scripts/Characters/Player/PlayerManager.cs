using UnityEngine;

namespace Group1{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] PlayerLocomotionManager locomotion;
        [HideInInspector] PlayerStatsManager playerStatsManager;

        [SerializeField] PlayerUIManager playerUIManager; 

        protected override void Awake()
        {
            base.Awake();   //runs CharacterManager Awake 

            locomotion = GetComponent<PlayerLocomotionManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            
            OnStaminaChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
            MaxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(endurance);
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(MaxStamina);
        }

        protected override void Update()
        {
            base.Update();

            //Handles movement
            locomotion.HandleMovement();
        }
    }       
}

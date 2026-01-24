using UnityEngine;

namespace Group1{
    public class PlayerManager : CharacterManager
    {
        PlayerLocomotionManager locomotion;
        protected override void Awake()
        {
            base.Awake();   //runs CharacterManager Awake 

            locomotion = GetComponent<PlayerLocomotionManager>();
            
        }

        protected override void Update()
        {
            base.Update();

            //Handles movement
            PlayerLocomotionManager.HandleMovement();
        }
    }       
}

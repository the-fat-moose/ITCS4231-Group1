using UnityEngine;

namespace Group1{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player; 

        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;

        private Vector3 moveDir;
        [SerializeField] float walkSpeed = 2;
        [SerializeField] float runSpeed = 5;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void HandleMovement()
        {
            HandleGroundMovement();
        }

        private void GetMovementInputs()
        {
            verticalMovement = PlayerInputManager.inputs.verticalInput;
            horizontalMovement = PlayerInputManager.inputs.horizontalInput;
        }

        private void HandleGroundMovement()
        {
            GetMovementInputs();
            //move dir is based on camera and inputs
            moveDir = PlayerCamera.cam.transform.forward * verticalMovement;
            moveDir = moveDir + PlayerCamera.cam.transform.right * horizontalMovement;
            moveDir.Normalize();
            moveDir.y = 0;

            if(PlayerInputManager.inputs.moveAmount > 0.5f)
            {
                //running
                player.characterController.Move(moveDir * runSpeed * Time.deltaTime);
            }
            else if(PlayerInputManager.inputs.moveAmount <= 0.5f)
            {
                //walking
                player.characterController.Move(moveDir * walkSpeed * Time.deltaTime);
            }
        }
    }
}

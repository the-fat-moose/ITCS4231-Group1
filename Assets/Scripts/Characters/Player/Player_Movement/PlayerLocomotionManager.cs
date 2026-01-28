using UnityEngine;

namespace Group1{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player; 

        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("Movement Settings")]
        private Vector3 moveDir;
        private Vector3 targetRotation;
        [SerializeField] float walkSpeed = 2f;
        [SerializeField] float runSpeed = 5f;
        [SerializeField] float rotationSpeed = 15f;

        [Header("Dodge")]
        private Vector3 rollDirection;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void HandleMovement()
        {
            HandleGroundMovement();
            HandleRotation();
        }

        private void GetMovementInputs()
        {
            verticalMovement = PlayerInputManager.inputs.verticalInput;
            horizontalMovement = PlayerInputManager.inputs.horizontalInput;
        }

        private void HandleGroundMovement()
        {
            GetMovementInputs();
            if(!player.canMove) return;
            
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

        private void HandleRotation()
        {
            if(!player.canRotate) return;
            targetRotation = Vector3.zero;
            targetRotation = PlayerCamera.cam.cameraObject.transform.forward *verticalMovement;
            targetRotation = targetRotation + PlayerCamera.cam.cameraObject.transform.right * horizontalMovement;
            targetRotation.Normalize();
            targetRotation.y = 0;

            if(targetRotation == Vector3.zero)
            {
                targetRotation = transform.forward;
            } 

            Quaternion newRotation = Quaternion.LookRotation(targetRotation);
            Quaternion targetRotationTurn = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotationTurn;
        }

        public void AttemptToDodge()
        {
            if(player.isPerformingAction) return;

            if(PlayerInputManager.inputs.moveAmount > 0)
            {
                rollDirection = PlayerCamera.cam.cameraObject.transform.forward * PlayerInputManager.inputs.verticalInput;
                rollDirection += PlayerCamera.cam.cameraObject.transform.right * PlayerInputManager.inputs.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();
                
                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                player.playerAnimatorManager.PlayTargetActionAnimation("PlayerCharacter_Dodge", true, true);
            }
            else
            {
                //backstep (not moving)
            }
        }
    }
}

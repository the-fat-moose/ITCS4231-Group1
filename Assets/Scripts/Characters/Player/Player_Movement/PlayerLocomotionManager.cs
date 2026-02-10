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
        [SerializeField] float sprintingSpeed = 7f;

        [Header("Jump")]
        [SerializeField] float jumpHeight = 2f;
        [SerializeField] float jumpForwardSpeed = 5f;
        [SerializeField] float freeFallingSpeed = 2f;
        private Vector3 jumpDirection;

        [Header("Stamina")]
        [SerializeField] int sprintingStaminaCost = 2;
        [SerializeField] int dodgeStaminaCost = 15;
        [SerializeField] int jumpStaminaCost = 15;

        [Header("Dodge")]
        [SerializeField] float rollSpeed = 6f;
        private Vector3 rollDirection;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void HandleMovement()
        {

            if (isRolling)
            {
                player.characterController.Move(rollDirection * rollSpeed * Time.deltaTime);
                return;
            }

            HandleGroundMovement();
            HandleRotation();
            HandleJumpMovement();
            HandleFreeFallMovement();
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
            moveDir = PlayerCamera.cam.cameraObject.transform.forward * verticalMovement;
            moveDir += PlayerCamera.cam.cameraObject.transform.right * horizontalMovement;
            moveDir.Normalize();
            moveDir.y = 0;

            if (player.isSprinting)
            {
                player.characterController.Move(moveDir * sprintingSpeed * Time.deltaTime);
            }
            else
            {
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

        private void HandleJumpMovement()
        {
            if (player.isJumping)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }

        private void HandleFreeFallMovement()
        {
            if (!player.isGrounded)
            {
                Vector3 freeFallDirection;
                freeFallDirection = PlayerCamera.cam.transform.forward * PlayerInputManager.inputs.verticalInput;
                freeFallDirection = freeFallDirection + PlayerCamera.cam.transform.right * PlayerInputManager.inputs.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection * freeFallingSpeed * Time.deltaTime);
            }
        }

        private void HandleRotation()
        {
            if(player.isDead) return;

            if(!player.canRotate) return;

            if (player.isLockedOn)
            {
                if (player.isSprinting || isRolling)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = PlayerCamera.cam.cameraObject.transform.forward * verticalMovement;
                    targetDirection += PlayerCamera.cam.cameraObject.transform.right * horizontalMovement;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if(targetDirection == Vector3.zero) targetDirection = transform.forward;

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
                else
                {
                    if(player.playerCombatManager.currentTarget == null) return;

                    Vector3 targetDirection;
                    targetDirection = player.playerCombatManager.currentTarget.transform.position - transform.position;
                    targetDirection.y = 0;
                    targetDirection.Normalize();

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
            }
            else
            {
                targetRotation = Vector3.zero;
                targetRotation = PlayerCamera.cam.cameraObject.transform.forward * verticalMovement;
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

        }

        public void HandleSprinting()
        {
            Debug.Log("HandleSprinting called");
            if (player.isPerformingAction)
            {
                Debug.Log("isPerformingAction");
                player.isSprinting = false;
            }

            if (player.CurrentStamina <= 0)
            {
                player.isSprinting = false;
                return;
            }

            if(PlayerInputManager.inputs.moveAmount >= 0.5)
            {
                Debug.Log("isSprinting set to true");
                player.isSprinting = true;
            }
            else
            {
                player.isSprinting = false;
            }

            if (player.isSprinting)
            {
                player.CurrentStamina -= sprintingStaminaCost * Time.deltaTime;
            }
        }

        public void AttemptToDodge()
        {
            if(player.isPerformingAction) return;

            if (player.CurrentStamina <= 0) return;

            if(PlayerInputManager.inputs.moveAmount > 0)
            {
                rollDirection = PlayerCamera.cam.cameraObject.transform.forward * PlayerInputManager.inputs.verticalInput;
                rollDirection += PlayerCamera.cam.cameraObject.transform.right * PlayerInputManager.inputs.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                /*if (rollDirection == Vector3.zero)
                {
                    rollDirection = transform.forward;

                }*/
                
                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                isRolling = true;

                player.playerAnimatorManager.PlayTargetActionAnimation("PlayerCharacter_Dodge", true, true);
                isRolling = true;
            }
            else
            {
                player.playerAnimatorManager.PlayTargetActionAnimation("PlayerCharacter_BackStep", true, true);
            }

            player.CurrentStamina -= dodgeStaminaCost;
        }

        public void AttemptToJump()
        {
            if(player.isPerformingAction) return;

            if(player.CurrentStamina <= 0) return;

            if(player.isJumping) return;

            if(!player.isGrounded) return;

            player.playerAnimatorManager.PlayTargetActionAnimation("PlayerCharacter_Jump", false);

            player.isJumping = true;

            player.CurrentStamina -= jumpStaminaCost;

            jumpDirection = PlayerCamera.cam.cameraObject.transform.forward * PlayerInputManager.inputs.verticalInput;
            jumpDirection += PlayerCamera.cam.cameraObject.transform.right * PlayerInputManager.inputs.horizontalInput;
            jumpDirection.y = 0;

            if(jumpDirection != Vector3.zero)
            {
                if(player.isSprinting)
                {
                    jumpDirection *= 1;
                }
                else if(PlayerInputManager.inputs.moveAmount > 0.5)
                {
                    jumpDirection *= 0.65f;
                }
                else if(PlayerInputManager.inputs.moveAmount <= 0.5)
                {
                    jumpDirection *= 0.25f;
                }
            }
        }

        public void ApplyJumpVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
    }
}

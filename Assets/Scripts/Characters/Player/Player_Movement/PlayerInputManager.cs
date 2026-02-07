using UnityEngine;

namespace Group1{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager inputs;
        public PlayerManager player;
        PlayerControls playerControls;

        [Header("Player Movement Input")]
        [SerializeField] Vector2 movement;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;
        [HideInInspector] public Vector3 dodgeDirection;
        [HideInInspector] public bool isDodging;

        [Header ("Player Action Input")]
        [SerializeField] private bool dodgeInput = false;
        [SerializeField] private bool sprintInput = false;
        [SerializeField] private bool jumpInput = false;
        [SerializeField] private bool RB_Input = false;

        [Header("Camera Movement Input")]
        [SerializeField] Vector2 camMovement;
        [SerializeField] private bool lockInput = false;
        public float verticalCameraInput;
        public float horizontalCameraInput;
        

        private void Awake()
        {
            if(inputs == null)
            {
                inputs = this;
            }
            else
            {
                Destroy(gameObject);
            } 

            
        }

        private void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movement = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => camMovement = i.ReadValue<Vector2>();
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;
                playerControls.PlayerActions.LockOn.performed += i => lockInput = true;
                playerControls.PlayerActions.LockOn.canceled += _ => lockInput = false;
                
                //holding activates
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                //release deactivates
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            }

            playerControls.Enable();
        }

        private void Update()
        {
            HandleAllInputs();
            if(lockInput) Debug.Log("lockInput true");
        }

        private void HandleAllInputs()
        {
            MovementInput();
            HandleLockOnInput();
            HandleCameraInput();
            HandleDodgeInput();
            HandleSprinting();
            HandleJumpInput();
            HandleRBInput();
        }

        private void HandleLockOnInput()
        {
            if (player.isLockedOn)
            {
                if (player.playerCombatManager.currentTarget == null) return;

                if (player.playerCombatManager.currentTarget.isDead)
                {
                    player.isLockedOn = false;
                }
            }

            if (lockInput && player.isLockedOn)
            {
                lockInput = false;
                Debug.Log("PlayerInputManager ClearLockOnTarget called");
                PlayerCamera.cam.ClearLockOnTarget();
                player.isLockedOn = false;
                return;
            }

            if (lockInput && !player.isLockedOn)
            {
                lockInput = true;
                
                PlayerCamera.cam.HandleLocatingLockOnTargets();

                if(PlayerCamera.cam.nearestLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.cam.nearestLockOnTarget);
                    player.isLockedOn = true;
                }
            }
        }
        
        //movements
        private void MovementInput()
        {
            verticalInput = movement.y;
            horizontalInput = movement.x;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
            bool isWalkHeld = playerControls.PlayerMovement.Walk.ReadValue<float>() > 0.1f;

            if(isWalkHeld  && moveAmount > 0f) //lets player slow walk on keyboard
            {
                moveAmount = 0.5f;
            }

            //clamps movement to be smoother (optional)
            if(moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if(moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1f;
            }

            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0f, moveAmount, player.isSprinting);
        }

        private void HandleCameraInput()
        {
            verticalCameraInput = camMovement.y;
            horizontalCameraInput = camMovement.x;
        }
        
        //actions
        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;

                Vector3 inputDirection = player.transform.forward * verticalInput + player.transform.right * horizontalInput;

                if(inputDirection == Vector3.zero)
                {
                    inputDirection = player.transform.forward;
                }

                dodgeDirection = inputDirection.normalized;

                isDodging = true;

                //for future, no dodge when ui open
                player.locomotion.AttemptToDodge();
            }
        }

        private void HandleSprinting()
        {
            if (sprintInput)
            {
                player.locomotion.HandleSprinting();
            }
            else
            {
                player.isSprinting = false;
            }
        }

        private void HandleJumpInput()
        {
            if(jumpInput == true)
            {
                jumpInput  = false;

                player.locomotion.AttemptToJump();
            }
        }
    
        private void HandleRBInput()
        {
            if (RB_Input)
            {
                RB_Input = false;

                // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.rb_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }
    }
}

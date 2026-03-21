using System.Collections;
using Unity.VisualScripting;
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
        [SerializeField] private bool switch_Weapon_Input = false;
        [SerializeField] private bool interaction_Input = false;

        [Header("Camera Movement Input")]
        [SerializeField] Vector2 camMovement;
        public float verticalCameraInput;
        public float horizontalCameraInput;

        [Header("Lock On")]
        [SerializeField] private bool lockInput = false;
        [SerializeField] private bool lockOn_Left = false;
        [SerializeField] private bool lockOn_Right = false;
        private Coroutine lockOnCoroutine;

        [Header("Qued inputs")]
        private bool input_Que_Is_Active = false;
        [SerializeField] float default_Que_Input_Timer = 0.35f;
        [SerializeField] float que_Input_Timer = 0;
        [SerializeField] bool que_RB_input = false;
        [SerializeField] bool que_RT_input = false;

        [Header("UI INPUTS")]
        [SerializeField] bool openCharacterMenuInput = false;
        [SerializeField] bool closeMenuInput = false;

        [Header("Bumper inputs")]
        [SerializeField] private bool RB_Input = false;
        [SerializeField] private bool LB_Input = false;

        [Header("Trigger inputs")]
        [SerializeField] bool RT_Input = false;
        [SerializeField] bool Hold_RT_Input = false;

        [Header("Blocking flags")]
        [SerializeField] bool stillBlocking = false;

        [Header("Ability Inputs")]
        [SerializeField] bool pushAbility_Input = false;
        [SerializeField] bool cageAbility_Input = false;

        private void Awake()
        {
            if(inputs == null)
            {
                inputs = this;
                DontDestroyOnLoad(gameObject);
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

                // ACTIONS
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControls.PlayerActions.SwitchWeapon.performed += i => switch_Weapon_Input = true;
                playerControls.PlayerActions.Interact.performed += i => interaction_Input = true;
                
                // BUMPERS
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;
                playerControls.PlayerActions.LB.performed += i => LB_Input = true;
                playerControls.PlayerActions.LB.canceled += i => LB_Input = false;
                
                // TRIGGERS
                playerControls.PlayerActions.RT.performed += i => RT_Input = true;
                playerControls.PlayerActions.HoldRT.performed += i => Hold_RT_Input = true;
                playerControls.PlayerActions.HoldRT.canceled += i => Hold_RT_Input = false;

                // LOCK ON
                playerControls.PlayerActions.LockOn.performed += i => lockInput = true;
                playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_Left = true;
                playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOn_Right = true;
                
                //holding activates
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                //release deactivates
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

                //Qued inputs
                playerControls.PlayerActions.QueRB.performed += i => QueInput(ref que_RB_input);
                playerControls.PlayerActions.QueRT.performed += i => QueInput(ref que_RT_input);

                //Ability inputs
                playerControls.PlayerActions.PushAbility.performed += i => pushAbility_Input = true;
                playerControls.PlayerActions.CageAbility.performed += i => cageAbility_Input = true;

                // UI Inputs
                playerControls.PlayerActions.Dodge.performed += i => closeMenuInput = true;
                playerControls.PlayerActions.OpenCharacterMenu.performed += i => openCharacterMenuInput = true;
            }

            playerControls.Enable();
        }

        private void Update()
        {
            HandleAllInputs();

        }

        private void HandleAllInputs()
        {
            if (player != null) 
            {
                MovementInput();
                HandleLockOnInput();
                HandleLockOnSwitchTargetInput();
                HandleCameraInput();
                HandleDodgeInput();
                HandleSprinting();
                HandleJumpInput();
                HandleRBInput();
                HandleLBInput();
                HandleRTInput();
                HandleHoldRTInput();
                HandleSwitchWeaponInput();
                HandleAllQuedInputs();
                HandleInteractionInput();
                HandleCloseUIInput();
                HandleOpenCharacterMenuInput();

                //abilites
                HandlePushAbilityInput();
                HandleCageAbilityInput();
            }
        }

        private void HandleLockOnInput()
        {
            if (!lockInput) return;

            lockInput = false; //consume input immediately

            if (player.isLockedOn)
            {
                if(player.playerCombatManager.currentTarget.isDead) player.isLockedOn = false;
                
                Debug.Log("PlayerInputManager ClearLockOnTarget called");
                PlayerCamera.cam.ClearLockOnTarget();
                player.isLockedOn = false;

                if(lockOnCoroutine != null) StopCoroutine(lockOnCoroutine);
                lockOnCoroutine = StartCoroutine(PlayerCamera.cam.WaitThenFindNewTarget());

                return;
            }


            PlayerCamera.cam.HandleLocatingLockOnTargets();

            if (PlayerCamera.cam.nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(PlayerCamera.cam.nearestLockOnTarget);
                player.isLockedOn = true;
            }
        }

        private void HandleLockOnSwitchTargetInput()
        {
            if (lockOn_Left)
            {
                lockOn_Left = false;

                if (player.isLockedOn)
                {
                    PlayerCamera.cam.HandleLocatingLockOnTargets();

                    if(PlayerCamera.cam.leftLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.cam.leftLockOnTarget);
                    }
                }
            }

            if (lockOn_Right)
            {
                lockOn_Right = false;

                if (player.isLockedOn)
                {
                    PlayerCamera.cam.HandleLocatingLockOnTargets();

                    if(PlayerCamera.cam.rightLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.cam.rightLockOnTarget);
                    }
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

            if(player == null) return;

            if (moveAmount != 0)
            {
                player.IsMoving = true;
            }
            else
            {
                player.IsMoving = false;
            }

            if (!player.isLockedOn)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0f, moveAmount, player.isSprinting);
            }
            else
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.isSprinting);
            }

            
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

                // no dodge when ui open
                if (PlayerUIManager.instance.menuWindowIsOpen) return;

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

                // no jump when ui open
                if (PlayerUIManager.instance.menuWindowIsOpen) return;

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

        private void HandleLBInput()
        {
            if (LB_Input)
            {
                if(!player.IsBlocking)
                {
                    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.lb_Action, player.playerInventoryManager.currentRightHandWeapon);
                }
            }
            else
            {
                if(player.IsBlocking)
                {
                    player.IsBlocking = false;
                }
            }
        }

        private void HandleRTInput()
        {
            if (RT_Input)
            {
                RT_Input = false;

                // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.rt_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        private void HandleHoldRTInput()
        {
            if (player.isPerformingAction)
            {
                player.Charging = Hold_RT_Input;
            }
        }

        private void HandleSwitchWeaponInput()
        {
            if (switch_Weapon_Input)
            {
                switch_Weapon_Input = false;

                if (PlayerUIManager.instance.menuWindowIsOpen) return;

                player.playerEquipmentManager.SwitchRightWeapon();
            }
        }

        private void HandlePushAbilityInput()
        {
            if (pushAbility_Input)
            {
                pushAbility_Input = false;

                // TODO: IF WE HAVE A UI WINDOW OPEN, RETURN AND DO NOTHING
                player.PerformAbilityAction(player.playerInventoryManager.pushAbility.actionID);           
            }
        }

        private void HandleCageAbilityInput()
        {
            if (cageAbility_Input)
            {
                cageAbility_Input = false;

                player.PerformAbilityAction(player.playerInventoryManager.cageAbility.actionID); 
            }
        }

        private void HandleInteractionInput()
        {
            if (interaction_Input)
            {
                interaction_Input = false;

                // CLOSE POP UP
                player.playerInteractionManager.Interact();
            }
        }

        private void QueInput(ref bool quedInput)   //using ref passes the bool object and not just the value, this lets us manipulate the bool from this method
        {
            que_RB_input = false;
            que_RT_input = false;

            if(player != null && !player.playerCombatManager.canComboWithMainHandWeapon)
            {
                quedInput = true;
                que_Input_Timer = default_Que_Input_Timer;
                input_Que_Is_Active = true;
            }
        }

        private void ProcessQuedInputs()
        {
            if(player.isDead) return;
            
            if(que_RB_input) RB_Input = true;
            if(que_RT_input) RT_Input = true;
        }

        private void HandleAllQuedInputs()
        {
            if (input_Que_Is_Active)
            {
                if(que_Input_Timer > 0)
                {
                    que_Input_Timer -= Time.deltaTime;
                    ProcessQuedInputs();
                }
                else
                {
                    que_RB_input = false;
                    que_RT_input = false;
                    input_Que_Is_Active = false;
                    que_Input_Timer = 0;
                }
            }
        }

        private void HandleOpenCharacterMenuInput()
        {
            if (openCharacterMenuInput)
            {
                openCharacterMenuInput = false;

                PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
                PlayerUIManager.instance.CloseAllMenuWindows();
                PlayerUIManager.instance.playerUICharacterMenuManager.OpenCharacterMenu();
            }
        }

        private void HandleCloseUIInput()
        {
            if (closeMenuInput)
            {
                closeMenuInput = false;

                if (PlayerUIManager.instance.menuWindowIsOpen)
                {
                    PlayerUIManager.instance.CloseAllMenuWindows();
                }
            }
        }
    }
}

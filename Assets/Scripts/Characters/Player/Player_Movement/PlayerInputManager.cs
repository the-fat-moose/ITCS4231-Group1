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

        [Header("Camera Movement Input")]
        [SerializeField] Vector2 camMovement;
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
                //holding activates
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                Debug.Log("sprintInput called");
                //release deactivates
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            }

            playerControls.Enable();
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            MovementInput();
            HandleCameraInput();
            HandleDodgeInput();
            HandleSprinting();
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

    }
}

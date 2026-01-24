using UnityEngine;

namespace Group1{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager inputs;
        PlayerControls playerControls;

        [SerializeField] Vector2 movement;
        [SerializeField] float verticalInput;
        [SerializeField] float horizontalInput;
        [SerializeField] public float moveAmount;

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
            }

            playerControls.Enable();
        }

        private void MovementInput()
        {
            verticalInput = movement.y;
            horizontalInput = movement.x;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            //clamps movement to be smoother (optional)
            if(moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if(moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1f;
            }
        }
}
}

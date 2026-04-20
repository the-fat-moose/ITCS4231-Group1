using UnityEngine;

namespace Group1{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;
        
        [Header("Ground & jumping check")]
        [SerializeField] protected float gravityForce = -5.55f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity;
        [SerializeField] protected float groundedYVelocity = -20;
        [SerializeField] protected float fallStartYVelocity = -5;
        protected bool fallingVelocityHasBeenSet = false;
        [HideInInspector] public bool inKnockUpAbility = false;
        protected float inAirTimer = 0;

        [Header("Flags")]
        public bool isRolling = false;
        public bool canRoll = true;
        public bool isRidingLift = false;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();

            if (inKnockUpAbility)
            {
                yVelocity.y = 0; // no gravity
                return;          // do NOT move the character controller
            }

            if (character.isGrounded)
            {
                if(yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                if(!character.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer += Time.deltaTime;
                character.animator.SetFloat("inAirTimer", inAirTimer);
                yVelocity.y += gravityForce * Time.deltaTime;
                
            }

            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }

        public void EnableCanRotate()
        {
            character.canRotate = true;
        }

        public void DisableCanRotate()
        {
            character.canRotate = false;
        }
    }
}
using Group1;
using UnityEngine;
namespace Group1{
    public class CharacterAnimatorManager : MonoBehaviour
    {

        public CharacterManager character;

        int vertical;
        int horizontal;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }


        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            float snappedHorizontal;
            float snappedVertical;

            if(horizontalMovement > 0 && horizontalMovement <= 0.5f)
            {
                snappedHorizontal = 0.5f;
            }
            else if(horizontalMovement > 0.5f && horizontalMovement <= 1f)
            {
                snappedHorizontal = 1f;
            }
            else if(horizontalMovement < -0f && horizontalMovement >= -0.5f)
            {
                snappedHorizontal = -0.5f;
            }
            else if(horizontalMovement < -0.5f && horizontalMovement >= -1f)
            {
                snappedHorizontal = -1f;
            }
            else
            {
                snappedHorizontal = 0f;
            }

            if(verticalMovement > 0 && verticalMovement <= 0.5f)
            {
                snappedVertical = 0.5f;
            }
            else if(verticalMovement > 0.5f && verticalMovement <= 1f)
            {
                snappedVertical = 1f;
            }
            else if(verticalMovement < 0f && verticalMovement >= -0.5f)
            {
                snappedVertical = -0.5f;
            }
            else if(verticalMovement < -0.5f && verticalMovement >= -1f)
            {
                snappedVertical = -1f;
            }
            else
            {
                snappedVertical = 0f;
            }

            if(isSprinting)
            {
                snappedVertical = 2f;
            }

            character.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
        }
    
        public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);

            //used to stop player from performing actions while in action
            character.isPerformingAction = isPerformingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;
        }

        public virtual void PlayTargetAttackActionAnimation(AttackType attackType, string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
        {
            int layerIndex = character.animator.GetLayerIndex("Action Override");

            Debug.Log(character.animator.HasState(
                character.animator.GetLayerIndex("Action Override"),
                Animator.StringToHash(targetAnimation)
            ));

            //used to stop player from performing actions while in action
            character.isPerformingAction = isPerformingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;

            character.animator.CrossFade(targetAnimation, 0.2f, layerIndex);

            //need to keep track of last attack performed for combo's
            character.characterCombatManager.currentAttackType = attackType;
            character.characterCombatManager.lastAttackAnimationPerformed = targetAnimation;
            character.applyRootMotion = applyRootMotion;
        }

        public virtual void EnableCanDoCombo()
        {

        }

        public virtual void DisableCanDoCombo()
        {

        }

    }
}

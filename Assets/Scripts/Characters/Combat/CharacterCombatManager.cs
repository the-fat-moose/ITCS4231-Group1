using UnityEngine;
using UnityEngine.TextCore.Text;
using NaughtyAttributes;
using Unity.VisualScripting;

namespace Group1 {
    public class CharacterCombatManager : MonoBehaviour
    {
        protected CharacterManager character;

        public bool canBlock = true;
        public bool canParry = false;

        public float parryWindow = 0.3f;

        public float baseMagicDamageMultiplier = 1f;
        public float magicDamageMultiplier = 1f;
        

        [Header("Last Attack Animation Performed")]
        public string lastAttackAnimationPerformed;
        
        [Header("Attack Target")]
        public CharacterManager currentTarget;
        [Header("Attack Type")]
        public AttackType currentAttackType;
        [Header("Lock On Transform")]
        [Required]
        public Transform lockOnTransform;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
        }

        public virtual void SetTarget(CharacterManager newTarget)
        {
            // TURN OFF OLD TARGET ICON
            if (currentTarget != null)
            {
                var oldIcon = currentTarget.characterCombatManager.lockOnTransform.gameObject.GetComponent<LockOnTransform>().lockOnIcon;
                if (oldIcon != null)
                    oldIcon.SetActive(false);
            }

            // UPDATE TARGET
            currentTarget = newTarget;

            // TURN ON NEW TARGET ICON
            if (currentTarget != null)
            {
                var newIcon = currentTarget.characterCombatManager.lockOnTransform.gameObject.GetComponent<LockOnTransform>().lockOnIcon;
                if (newIcon != null)
                    newIcon.SetActive(true);
            }
        }
    
        public void EnableIsInvulnerable()
        {
            character.isInvulnerable = true;
        }

        public void DisableIsInvulnerable()
        {
            character.isInvulnerable = false;
        }
    }
}
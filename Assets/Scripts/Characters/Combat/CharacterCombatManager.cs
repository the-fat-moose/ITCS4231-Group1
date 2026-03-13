using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Group1 {
    public class CharacterCombatManager : MonoBehaviour
    {
        protected CharacterManager character;

        public bool canBlock = true;
        public bool canParry = false;

        public float parryWindow = 0.3f;
        

        [Header("Last Attack Animation Performed")]
        public string lastAttackAnimationPerformed;
        
        [Header("Attack Target")]
        public CharacterManager currentTarget;
        [Header("Attack Type")]
        public AttackType currentAttackType;
        [Header("Lock On Transform")]
        public Transform lockOnTransform;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
        }

        public virtual void SetTarget(CharacterManager newTarget)
        {
            if(newTarget != null)
            {
                currentTarget = newTarget;
            }
            else
            {
                currentTarget = null;
            }
        }
    }
}
using Unity.VisualScripting;
using UnityEngine;

namespace Group1 {
    [CreateAssetMenu(menuName = "AI/Actions/Attack")]
    public class AICharacterAttackAction : ScriptableObject
    {
        [Header("Attack")]
        [SerializeField] private string attackAnimation;

        [Header("Combo Action")]
        public AICharacterAttackAction comboAction; // The combo action of this attack action

        [Header("Action Values")]
        public int attackWeight = 50;
        [SerializeField] AttackType attackType;
        // ATTACK CAN BE REPEATED
        public float actionRecoveryTime = 1.5f; // the time before the character can make another attack after performing this one
        public float minimumAttackAngle = -35f;
        public float maximumAttackAngle = 35f;
        public float minimumAttackDistance = 0f;
        public float maximumAttackDistance = 2f;

        public void AttemptToPerformAction(AICharacterManager aiCharacter)
        {
            aiCharacter.characterAnimatorManager.PlayTargetAttackActionAnimation(attackType, attackAnimation, true);
        }
    }
}
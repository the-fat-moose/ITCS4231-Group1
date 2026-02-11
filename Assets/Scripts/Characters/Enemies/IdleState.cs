using UnityEngine;

namespace Group1 {
    [CreateAssetMenu(menuName = "AI/States/Idle")]
    public class IdleState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.characterCombatManager.currentTarget != null)
            {
                // RETURN THE PURSUE TARGET STATE
                Debug.Log("WE HAVE A TARGET");

                return this;
            }
            else
            {
                // RETURN THIS STATE, TO CONTINUALLY SEARCH FOR A TARGET
                aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);

                return this;
            }
        }
    }
}
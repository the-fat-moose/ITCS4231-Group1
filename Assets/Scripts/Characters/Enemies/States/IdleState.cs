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
                Debug.Log("Enemy AI State: Idle State: WE HAVE A TARGET");
                SwitchState(aiCharacter, aiCharacter.pursueTarget);
                return this;
            }
            else
            {
                // RETURN THIS STATE, TO CONTINUALLY SEARCH FOR A TARGET
                aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);
                Debug.Log("Enemy AI State: Idle State: SEARCHING FOR A TARGET");

                return this;
            }
        }
    }
}
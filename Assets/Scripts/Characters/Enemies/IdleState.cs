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
            }
            else
            {
                // RETURN THIS STATE, TO CONTINUALLY SEARCH FOR A TARGET
                Debug.Log("WE HAVE NO TARGET");
            }

            return this;
        }
    }
}
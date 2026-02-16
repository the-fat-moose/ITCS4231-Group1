using UnityEngine;
using UnityEngine.AI;

namespace Group1 {
    [CreateAssetMenu(menuName = "AI/States/Pursue Target")]
    public class PursueTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            // CHECK IF WE ARE PERFORMING AN ACTION (IF SO DO NOTHING UNTIL ACTION IS COMPLETE)
            if (aiCharacter.isPerformingAction) return this;

            // CHECK IF OUR TARGET IS NULL, IF SO, RETURN TO IDLE
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null) return SwitchState(aiCharacter, aiCharacter.idle);

            // MAKE SURE OUR NAV MESH AGENT IS ACTIVE; IF ITS NOT, ENABLE IT
            if (!aiCharacter.navMeshAgent.enabled) aiCharacter.navMeshAgent.enabled = true;

            // IF WE ARE WITHIN COMBAT RANGE, SWITCH TO A COMBAT STANCE STATE

            // IF THE TARGET IS NOT REACHABLE, RETURN HOME

            // PURSUE THE TARGET
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }
    }
}
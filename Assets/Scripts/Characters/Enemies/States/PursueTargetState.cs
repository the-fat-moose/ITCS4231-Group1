using UnityEngine;

namespace Group1 {
    [CreateAssetMenu(menuName = "AI/States/Pursue Target")]
    public class PursueTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            Debug.Log("PURSUE TARGET");
            return base.Tick(aiCharacter);

            // CHECK IF WE ARE PERFORMING AN ACTION (IF SO DO NOTHING UNTIL ACTION IS COMPLETE)

            // CHECK IF OUR TARGET IS NULL, IF SO, RETURN TO IDLE

            // MAKE SURE OUR NAV MESH AGENT IS ACTIVE; IF ITS NOT, ENABLE IT

            // IF WE ARE WITHIN COMBAT RANGE, SWITCH TO A COMBAT STANCE STATE

            // IF THE TARGET IS NOT REACHABLE, RETURN HOME

            // PURSUE THE TARGET
        }
    }
}
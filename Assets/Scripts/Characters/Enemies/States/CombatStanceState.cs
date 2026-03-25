using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Group1 {
    [CreateAssetMenu(menuName = "AI/States/Combat Stance")]
    public class CombatStanceState : AIState
    {
        // Select an attack for the attack state, depending on distance and angle of target in relation to character
        [Header("Attacks")]
        public List<AICharacterAttackAction> aiCharacterAttacks; // a list of all possible attacks this character can do
        protected List<AICharacterAttackAction> potentialAttacks; // a list that is created during this state, all attacks possible in this situation (based on angle, distance, etc)
        private AICharacterAttackAction chosenAttack;
        private AICharacterAttackAction previousAttack;
        protected bool hasAttack = false;

        [Header("Combo")]
        [SerializeField] protected bool canPerformCombo = false; // if the character can perform a combo attack, after the initial attack
        [SerializeField] protected int chanceToPerformCombo = 25; // The chance (in percent) of the character to perform a combo on the next attack
        protected bool hasRolledForComboChance = false; // if we have already rolled for the chance during this state

        [Header("Pivot")]
        [SerializeField] protected bool enablePivot;

        [Header("Engagement Distance")]
        [SerializeField] public float maximumEngagementDistance = 5f; // the distance we have to be away from the target before we enter the pursue target state

        // process any combat logic while waiting to attack
        // if targets moves out of combat range, switch to pursue target state
        // if the target is no longer present, switch to idle state

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction) return this;

            if (!aiCharacter.navMeshAgent.enabled) aiCharacter.navMeshAgent.enabled = true;

            // MAKE THE AI CHARACTER TURN AND TURN TOWARDS ITS TARGET WHEN ITS OUTSIDE ITS FOV
            if (aiCharacter.aiCharacterCombatManager.enablePivot)
            {
                if (!aiCharacter.IsMoving)
                {
                    if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                        aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
                }
            }

            // ROTATE TO FACE OUR TARGET
            aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

            if (aiCharacter.aiCharacterCombatManager.currentTarget == null) return SwitchState(aiCharacter, aiCharacter.idle);

            // IF WE DO NOT HAVE AN ATTACK, GET ONE
            if (!hasAttack) 
            { 
                GetNewAttack(aiCharacter); 
            }
            else
            {
                // PASS ATTACK TO ATTACK STATE
                aiCharacter.attack.currentAttack = chosenAttack;
                // ROLL FOR COMBO CHANCE
                // SWITCH STATE
                return SwitchState(aiCharacter, aiCharacter.attack);
            }

            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance) return SwitchState(aiCharacter, aiCharacter.pursueTarget);

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }

        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            potentialAttacks = new List<AICharacterAttackAction>();

            // Sort through all possible attacks
            foreach(var potentialAttack in aiCharacterAttacks)
            {
                // Remove attacks that cant be used in a given situation
                if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget) continue; // too close

                if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget) continue; // too far

                if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle) continue; // outside minimum FOV
            
                if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle) continue; // outside maximum FOV

                // Place the remaining attacks into a list
                potentialAttacks.Add(potentialAttack);
            }

            if (potentialAttacks.Count <= 0) return;

            var totalWeight = 0;

            foreach(var attack in potentialAttacks)
            {
                totalWeight += attack.attackWeight;
            }
            
            var randomWeightValue = Random.Range(1, totalWeight + 1);
            var processedWeight = 0;

            foreach(var attack in potentialAttacks)
            {
                processedWeight += attack.attackWeight;

                // Pick one of the remaining attacks randomly
                if (randomWeightValue <= processedWeight)
                {
                    // Select this attack and pass it to the attack state
                    chosenAttack = attack;
                    previousAttack = chosenAttack;
                    hasAttack = true;
                    return;
                }
            }
        }

        protected virtual bool RollForOutcomeChance(int outcomeChance)
        {
            bool outcomeWillBePerformed = false;

            int randomPercentage = Random.Range(0, 100);

            if (randomPercentage < outcomeChance) outcomeWillBePerformed = true;

            return outcomeWillBePerformed;
        }

        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);

            hasRolledForComboChance = false;
            hasAttack = false;
        }
    }
}
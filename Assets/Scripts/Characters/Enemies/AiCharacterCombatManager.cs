using UnityEngine;

namespace Group1 {
    public class AiCharacterCombatManager : CharacterCombatManager
    {
        [Header("Action Recovery")]
        public float actionRecoveryTimer = 0;

        [Header("Target Information")]
        public float viewableAngle;
        public Vector3 targetsDirection;
        public float distanceFromTarget;

        [Header("Detection")]
        [SerializeField] float detectionRadius = 15f;
        [SerializeField] float minimumDetectionAngle = -35;
        [SerializeField] float maximumDetectionAngle = 35;

        [Header("Attack Rotation Speed")]
        public float attackRotationSpeed = 25f;

        public void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
        {
            if (currentTarget != null) return;

            Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.Instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                if (targetCharacter == null) continue;

                if (targetCharacter == aiCharacter) continue;

                if (targetCharacter.isDead) continue;

                if (WorldUtilityManager.Instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
                {
                    // IF A POTENTIAL TARGET IS FOUND, IT HAS TO BE IN FRONT OF US
                    Vector3 targetDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                    float angleOfPotentialTarget = Vector3.Angle(targetDirection, aiCharacter.transform.forward);

                    if (angleOfPotentialTarget > minimumDetectionAngle && angleOfPotentialTarget < maximumDetectionAngle)
                    {
                        // CHECK FOR ENVIRONMENTAL BLOCKS
                        if (Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position, 
                                            targetCharacter.characterCombatManager.lockOnTransform.position, 
                                            WorldUtilityManager.Instance.GetEnviroLayers()))
                        {
                            Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                        }
                        else
                        {
                            targetsDirection = targetCharacter.transform.position - transform.position;
                            viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, targetsDirection);
                            aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                        }
                    }
                }
            }
        }
    
        public void RotateTowardsAgent(AICharacterManager aiCharacter)
        {
            if (aiCharacter.IsMoving)
            {
                aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
            }
        }

        public void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacter)
        {
            if (currentTarget == null) return;

            // CHECK IF WE CAN ROTATE
            if (!aiCharacter.canRotate) return;

            if (!aiCharacter.isPerformingAction) return;

            // ROTATE TOWARDS THE TARGET AT A SPECIFIED ROTATION SPEED DURING SPECIFIED FRAMES
            Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();

            if (targetDirection == Vector3.zero) targetDirection = aiCharacter.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
        }
        
        public void HandleActionRecovery(AICharacterManager aiCharacter)
        {
            if (actionRecoveryTimer > 0)
            {
                if (!aiCharacter.isPerformingAction)
                {
                    actionRecoveryTimer -= Time.deltaTime;
                }
            }
        }
    }
}
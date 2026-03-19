using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Group1 {
    public class AICharacterManager : CharacterManager
    {
        [HideInInspector] public AiCharacterCombatManager aiCharacterCombatManager;
        [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;

        [Header("Navmesh Agent")]
        public NavMeshAgent navMeshAgent;

        [Header("Current State")]
        [SerializeField] AIState currentState;

        [Header("States")]
        public IdleState idle;
        public PursueTargetState pursueTarget;
        public CombatStanceState combatStance;
        public AttackState attack;

        protected override void Awake()
        {
            base.Awake();

            aiCharacterCombatManager = GetComponent<AiCharacterCombatManager>();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();

            navMeshAgent = GetComponentInChildren<NavMeshAgent>();

            idle = Instantiate(idle);
            pursueTarget = Instantiate(pursueTarget);

            currentState = idle;
        }

        protected override void Update()
        {
            base.Update();

            aiCharacterCombatManager.HandleActionRecovery(this);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            ProcessStateMachine();
        }

        public void ApplyKnockback(Vector3 direction, float force, float duration = 0.2f)
        {
            StartCoroutine(KnockbackRoutine(direction, force, duration));
        }

        private IEnumerator KnockbackRoutine(Vector3 direction, float force, float duration)
        {
            Debug.LogError("KnockBackRoutine");
            // Stop AI movement
            if (navMeshAgent != null) navMeshAgent.enabled = false;

            // Disable root motion temporarily
            animator.applyRootMotion = false;

            float timer = 0f;

            while (timer < duration)
            {
                transform.position += direction * force * Time.deltaTime;
                timer += Time.deltaTime;
                yield return null;
            }

            // Re-enable AI
            if (navMeshAgent != null) navMeshAgent.enabled = true;

            // Re-enable root motion
            animator.applyRootMotion = true;
        }

        private void ProcessStateMachine()
        {
            AIState nextState = null;

            if (currentState != null)
            {
                nextState = currentState.Tick(this);
            }

            if (nextState != null)
            {
                currentState = nextState;
            }

            // THE POSITION/ROTATION SHOULD BE RESET ONLY AFTER THE STATE MACHINE HAS PROCESSED ITS TICK
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

            if (aiCharacterCombatManager.currentTarget != null)
            {
                aiCharacterCombatManager.targetsDirection = aiCharacterCombatManager.currentTarget.transform.position - transform.position;
                aiCharacterCombatManager.viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, aiCharacterCombatManager.targetsDirection);

                aiCharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aiCharacterCombatManager.currentTarget.transform.position);
            }

            if (navMeshAgent.enabled)
            {
                Vector3 agentDestination = navMeshAgent.destination;
                float remainingDistance = Vector3.Distance(agentDestination, transform.position);

                if (remainingDistance > navMeshAgent.stoppingDistance)
                {
                    IsMoving = true;
                }
                else
                {
                    IsMoving = false;
                }
            }
            else
            {
                IsMoving = false;
            }
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

namespace Group1 {
    public class AICharacterManager : CharacterManager
    {
        [HideInInspector] public AiCharacterCombatManager aiCharacterCombatManager;

        [Header("Navmesh Agent")]
        public NavMeshAgent navMeshAgent;

        [Header("Current State")]
        [SerializeField] AIState currentState;

        [Header("States")]
        public IdleState idle;
        public PursueTargetState pursueTarget;
        // COMBAT STANCE
        // ATTACK STANCE

        protected override void Awake()
        {
            base.Awake();

            aiCharacterCombatManager = GetComponent<AiCharacterCombatManager>();

            navMeshAgent = GetComponentInChildren<NavMeshAgent>();

            idle = Instantiate(idle);
            pursueTarget = Instantiate(pursueTarget);

            currentState = idle;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            ProcessStateMachine();

            Debug.Log("AICHARACTER.IsMoving: " + IsMoving);
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
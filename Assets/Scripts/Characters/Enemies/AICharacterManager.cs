using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Group1 {
    public class AICharacterManager : CharacterManager
    {
        [Header("Character Name")]
        public string characterName = "";

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

        protected override void Start()
        {
            base.Start();

            // UPDATE TOTAL AMOUNT OF HEALTH, STAMINA, MANA WHEN THE STAT LINKED TO EITHER CHANGES
            OnEnduranceChanged += SetNewMaxStaminaValue;
            OnVitalityChanged += SetNewMaxHealthValue;
            OnMindChanged += SetNewMaxManaValue;

            SetNewMaxHealthValue(0, Vitality);
            SetNewMaxManaValue(0, Mind);
            SetNewMaxStaminaValue(0, Endurance);

            // Death and Healing Handling
            OnHealthChanged += CheckHP;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (characterUIManager.hasFloatingHPBar)
                OnHealthChanged += characterUIManager.OnHPChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (characterUIManager.hasFloatingHPBar)
                OnHealthChanged -= characterUIManager.OnHPChanged;
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

    #region Abilites
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

        public void Freeze(float duration)
        {
            Debug.LogError("Freeze called");
            StartCoroutine(FreezeRoutine(duration));
        }

        private IEnumerator FreezeRoutine(float duration)
        {
            Debug.LogError("FreezeRoutine");
            if (navMeshAgent != null) navMeshAgent.enabled = false;

            animator.applyRootMotion = false;

            //Stop animations
            animator.speed = 0f;

            yield return new WaitForSeconds(duration);

            //Restore everything
            if (navMeshAgent != null) navMeshAgent.enabled = true;

            animator.applyRootMotion = true;
            animator.speed = 1f;
            Debug.LogError("FreezeRoutine End");
        }

        #endregion

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
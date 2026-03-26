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
        [SerializeField] protected AIState currentState;

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

            idle = Instantiate(idle);
            pursueTarget = Instantiate(pursueTarget);
            combatStance = Instantiate(combatStance);
            attack = Instantiate(attack);

            currentState = idle;
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

            OnHealthChanged -= CheckHP;
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

        public void KnockUp(float liftHeight, float floatDuration, float slamDamage)
        {
            StartCoroutine(KnockUpRoutine(liftHeight, floatDuration, slamDamage));
        }

        private IEnumerator KnockUpRoutine(float liftHeight, float floatDuration, float slamDamage)
        {
            Debug.LogError("KnockUpRoutine Start");
            characterLocomotionManager.inKnockUpAbility = true;
            // Disable AI movement
            if (navMeshAgent != null) navMeshAgent.enabled = false;

            // Disable root motion
            animator.applyRootMotion = false;

            // Disable gravity if using Rigidbody
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) rb.useGravity = false;

            // Play floating animation
            animator.CrossFade("PlayerCharacter_JumpIdle", 0.2f);

            Vector3 startPos = transform.position;
            Vector3 peakPos = startPos + Vector3.up * liftHeight;

            float t = 0f;

            // Lift up
            while (t < 1f)
            {
                transform.position = Vector3.Lerp(startPos, peakPos, t);
                t += Time.deltaTime * 2f;
                yield return null;
            }

            // Float at peak
            yield return new WaitForSeconds(floatDuration);

            // Slam down
            t = 0f;
            while (t < 1f)
            {
                transform.position = Vector3.Lerp(peakPos, startPos, t);
                t += Time.deltaTime * 3f;
                yield return null;
            }

            // Apply slam damage
            TakeDamageEffect dmg = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            dmg.magicDamage = slamDamage;
            characterEffectsManager.ProcessInstantEffect(dmg);

            // Re-enable gravity
            if (rb != null) rb.useGravity = true;

            // Restore AI
            if (navMeshAgent != null) navMeshAgent.enabled = true;

            animator.applyRootMotion = true;

            Debug.LogError("KnockUpRoutine End");
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Group1{
    public class CharacterManager : MonoBehaviour
    {
        public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;
        [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;

        public bool isSprinting = false;
        public bool isDead = false;
        public bool isLockedOn = false;
        private bool isMoving = false;

        [Header("Character Group")]
        public CharacterGroup characterGroup;

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool isGrounded = true;
        public bool isJumping = false;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;
        public bool isChargingAttack = false;

        public event System.Action<bool, bool> OnIsChargingAttackChanged;
        public event System.Action<bool, bool> OnIsMovingValueChanged;

        public bool IsMoving
        {
            get => isMoving;
            set
            {
                if (isMoving == value) return;

                bool oldValue = isMoving;
                isMoving = value;
                OnIsMovingValueChanged?.Invoke(oldValue, isMoving);
            }
        }

    #region Stat Variables
        [Header("Stats")]
        [SerializeField] private int endurance = 10;
        [SerializeField] private int vitality = 10;
        [SerializeField] private int mind = 10;

        public event System.Action<int, int> OnVitalityChanged;
        public event System.Action<int, int> OnEnduranceChanged;
        public event System.Action<int, int> OnMindChanged;

        public int Endurance
        {
            get => endurance;
            set
            {
                if (endurance == value) return;

                int oldValue = endurance;
                endurance = value;
                OnEnduranceChanged?.Invoke(oldValue, endurance);
            }
        }

        public int Vitality
        {
            get => vitality;
            set
            {
                if (vitality == value) return;

                int oldValue = vitality;
                vitality = value;
                OnVitalityChanged?.Invoke(oldValue, vitality);
            }
        }

        public int Mind
        {
            get => mind;
            set
            {
                if (mind == value) return;

                int oldValue = mind;
                mind = value;
                OnMindChanged?.Invoke(oldValue, mind);
            }
        }

        public bool Charging
        {
            get => isChargingAttack;
            set
            {
                if(Charging == value) return;

                bool oldStatus = isChargingAttack;
                isChargingAttack = value;
                OnIsChargingAttackChanged?.Invoke(oldStatus, isChargingAttack);
            }
        }

        public event System.Action<float, float> OnStaminaChanged;
        public event System.Action<int, int> OnHealthChanged;
        public event System.Action<int, int> OnManaChanged;

        [Header("Resources")]
        [SerializeField] private float currentStamina = 0;

        public float CurrentStamina
        {
            get => currentStamina;
            set
            {
                if (currentStamina == value) return;

                float oldValue = currentStamina;
                currentStamina = value;

                OnStaminaChanged?.Invoke(oldValue, currentStamina);
            }
        }

        [SerializeField] private int maxStamina = 0;

        public int MaxStamina
        {
            get => maxStamina;
            protected set => maxStamina = value;
        }

        [SerializeField] private int currentHealth = 0;

        public int CurrentHealth
        {
            get => currentHealth;
            set
            {
                if (currentHealth == value) return;

                int oldValue = currentHealth;
                currentHealth = value;

                OnHealthChanged?.Invoke(oldValue, currentHealth);
            }
        }

        [SerializeField] private int maxHealth = 0;

        public int MaxHealth
        {
            get => maxHealth;
            protected set => maxHealth = value;
        }

        [SerializeField] private int currentMana = 0;

        public int CurrentMana
        {
            get => currentMana;
            set
            {
                if (currentMana == value) return;

                int oldValue = currentMana;
                currentMana = value;
                OnManaChanged?.Invoke(oldValue, currentMana);
            }
        }

        [SerializeField] private int maxMana = 0;

        public int MaxMana
        {
            get => maxMana;
            protected set => maxMana = value;
        }
    
    #endregion

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();

            OnIsMovingValueChanged += OnIsMovingChanged;
        }

        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded);
        }

        protected virtual void FixedUpdate()
        {
            
        } 

        protected virtual void LateUpdate()
        {
            
        }

        public void OnIsLockedOnChanged(bool old, bool isLockedOn)
        {
            if (!isLockedOn)
            {
                characterCombatManager.currentTarget = null;
            }
        }

        public void OnIsMovingChanged(bool oldStatus, bool newStatus)
        {
            animator.SetBool("isMoving", IsMoving);
        }

        public void CheckHP(int oldValue, int newValue)
        {
            if (CurrentHealth <= 0)
            {
                StartCoroutine(ProcessDeathEvent());
            }

            // PREVENTS US FROM OVER HEALING
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        public virtual IEnumerator ProcessDeathEvent()
        {
            CurrentHealth = 0;
            isDead = true;

            // RESET ANY FLAGS HERE THAT NEED TO BE RESET
            // NOTHING YET

            //characterAnimatorManager.PlayTargetActionAnimation("Death", true);

            // PLAY SOME DEATH SFX

            yield return new WaitForSeconds(5f);
            
            // DISABLE CHARACTER
        }

        public virtual void ReviveCharacter()
        {
            
        }

        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();

            List<Collider> ignoreColliders = new List<Collider>();

            foreach (var collider in damageableCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }

            ignoreColliders.Add(characterControllerCollider);

            // GOES THROUGH EVERY COLLIDER ON THE LIST AND MAKES THEM IGNORE COLLISION WITH EACH OTHER
            foreach (var collider in ignoreColliders)
            {
                foreach (var otherCollider in ignoreColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollider, true);
                }
            }
        }
    }
}

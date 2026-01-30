using System.Collections;
using UnityEngine;

namespace Group1{
    public class CharacterManager : MonoBehaviour
    {
        public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;

        public bool isSprinting = false;

        public bool isDead = false;

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;

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
        }

        protected virtual void Update()
        {
            
        }

        protected virtual void FixedUpdate()
        {
            
        }

        protected virtual void LateUpdate()
        {
            
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
    }
}

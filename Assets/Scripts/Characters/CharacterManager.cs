using UnityEngine;

namespace Group1{
    public class CharacterManager : MonoBehaviour
    {
        public CharacterController characterController;
        [HideInInspector] public Animator animator;

    #region Stat Variables
        [Header("Stats")]
        [SerializeField] private int endurance = 1;
        [SerializeField] private int vitality = 1;
        [SerializeField] private int mind = 1;

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

        public event System.Action<int, int> OnStaminaChanged;
        public event System.Action<int, int> OnHealthChanged;
        public event System.Action<int, int> OnManaChanged;

        [Header("Resources")]
        [SerializeField] private int currentStamina = 0;

        public int CurrentStamina
        {
            get => currentStamina;
            set
            {
                if (currentStamina == value) return;

                int oldValue = currentStamina;
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
        }

        protected virtual void Update()
        {
            
        }

        protected virtual void LateUpdate()
        {
            
        }
    }
}

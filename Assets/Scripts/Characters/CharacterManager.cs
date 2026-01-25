using UnityEngine;

namespace Group1{
    public class CharacterManager : MonoBehaviour
    {
        public CharacterController characterController;
        [HideInInspector] public Animator animator;

        [Header("Stats")]
        [SerializeField] private int endurance = 1;
        [SerializeField] private int vitality = 1;

        public event System.Action<int, int> OnVitalityChanged;
        public event System.Action<int, int> OnEnduranceChanged;

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

        public event System.Action<int, int> OnStaminaChanged;
        public event System.Action<int, int> OnHealthChanged;

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

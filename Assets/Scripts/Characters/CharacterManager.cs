using UnityEngine;

namespace Group1{
    public class CharacterManager : MonoBehaviour
    {
        public CharacterController characterController;

        [Header("Stats")]
        public int endurance = 1;

        public event System.Action<int, int> OnStaminaChanged;

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

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
        }

        protected virtual void Update()
        {
            
        }

        protected virtual void LateUpdate()
        {
            
        }
    }
}

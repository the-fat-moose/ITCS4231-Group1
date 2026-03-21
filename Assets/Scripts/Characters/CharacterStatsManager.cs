using UnityEngine;

namespace Group1 {
    public class CharacterStatsManager : MonoBehaviour
    {
        private CharacterManager character;

        [Header("Stamina Regeneration")]
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] private float staminaRegenerationDelay = 2f;
        public int staminaRegenerationAmount = 2;

        public float blockingPhysicalAbsorption = 65f;
        public float blockingMagicAbsorption = 65f;

        

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start() {}

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            // FORMULA TO DETERMINE HOW STAMINA IS CALCULATED
            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }

        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            float health = 0;

            // FORMULA TO DETERMINE HOW HEALTH IS CALCULATED
            health = vitality * 15;

            return Mathf.RoundToInt(health);
        }

        public int CalculateManaBasedOnMindLevel(int mind)
        {
            float mana = 0;

            // FORMULA TO DETERMINE HOW MANA IS CALCULATED
            mana = mind * 10;

            return Mathf.RoundToInt(mana);
        }

        public virtual void RegenerateStamina()
        {
            // DO NOT REGENERATE STAMINA IF WE ARE USING IT
            if (character.isSprinting)
            {
                return;
            }

            if (character.isPerformingAction)
            {
                return;
            }

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (character.CurrentStamina < character.MaxStamina)
                {
                    staminaTickTimer += Time.deltaTime;

                    if (staminaTickTimer >= 0.1) // 1/10th of a second
                    {
                        staminaTickTimer = 0;
                        character.CurrentStamina += staminaRegenerationAmount;
                    }
                }
            }
        }
    
        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            // WE ONLY WANT TO RESET THE REGENERATION IF THE ACTION USED STAMINA
            // WE DONT WANT TO RESET THE REGENERATION IF WE ARE ALREADY REGENERATING STAMINA
            if (currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenerationTimer = 0;
            }
        }
    }
}
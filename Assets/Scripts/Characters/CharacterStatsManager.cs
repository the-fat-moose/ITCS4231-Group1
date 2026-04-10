using UnityEngine;

namespace Group1 {
    public class CharacterStatsManager : MonoBehaviour
    {
        private CharacterManager character;

        [Header("Health Modifier")]
        protected float baseHealthMultiplier = 1f;
        public float maxHealthMultiplier = 1f;

        [Header("Mana Modifier")]
        protected float baseManaMultiplier = 1f;
        public float maxManaMultiplier = 1f;

        [Header("Stamina Regeneration")]
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] private float staminaRegenerationDelay = 2f;
        public int staminaRegenerationAmount = 2;
        protected float baseStaminaRegenMultiplier = 1f;
        public float staminaRegenMultiplier = 1f;

        public float blockingPhysicalAbsorption = 65f;
        public float blockingMagicAbsorption = 65f;

        [Header("Mana Regeneration")]
        protected float baseManaRegenMultiplier = 1f;
        public float manaRegenMultiplier = 1f;

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
            health = vitality * maxHealthMultiplier * 15;

            return Mathf.CeilToInt(health);
        }

        public int CalculateManaBasedOnMindLevel(int mind)
        {
            float mana = 0;

            // FORMULA TO DETERMINE HOW MANA IS CALCULATED
            mana = mind * maxManaMultiplier * 10;

            return Mathf.CeilToInt(mana);
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
                        character.CurrentStamina += staminaRegenerationAmount * staminaRegenMultiplier;
                    }
                }
            }
        }

        public virtual void RegenerateMana()
        {
            float newMana = character.CurrentMana;
            newMana += Mathf.CeilToInt(15 * manaRegenMultiplier);
            character.CurrentMana = (int)Mathf.Clamp(newMana, 0, character.MaxMana);
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
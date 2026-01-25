using UnityEngine;

namespace Group1 {
    public class CharacterStatsManager : MonoBehaviour
    {
        protected virtual void Awake() {}

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
    }
}

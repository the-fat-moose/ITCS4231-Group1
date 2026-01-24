using UnityEngine;

namespace Group1 {
    public class CharacterStatsManager : MonoBehaviour
    {
        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            // FORMULA TO DETERMINE HOW STAMINA IS CALCULATED
            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }
    }
}

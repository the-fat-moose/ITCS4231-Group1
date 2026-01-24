using UnityEngine;

namespace Group1 {
    public class CharacterStatsManager : MonoBehaviour
    {
        public void CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            // FORMULA TO DETERMINE HOW STAMINA IS CALCULATED
            stamina = endurance * 10;
        }
    }
}

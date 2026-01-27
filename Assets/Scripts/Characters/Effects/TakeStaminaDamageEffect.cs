using UnityEngine;

namespace Group1 {
    [CreateAssetMenu(menuName = "Effects/Instant Effects/Take Stamina Damage")]
    public class TakeStaminaDamageEffect : InstantCharacterEffect
    {
        public float staminaDamage;

        public override void ProcessEffect(CharacterManager character)
        {
            CalculateStaminaDamage(character);
        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            // COMPARE THE STAMINA DAMAGE AGAINST OTHER EFFECTS AND MODIFIERS
            // CHANGE THE VALUE BEFORE SUBTRACTING/ADDING IT
            // PLAY SOUND FX OR VFX DURING EFFECT

            character.CurrentStamina -= Mathf.RoundToInt(staminaDamage);
        }
    }
}
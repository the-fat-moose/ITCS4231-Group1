using UnityEngine;

namespace Group1
{    
    [CreateAssetMenu(menuName = "Effects/Instant Effects/Knock Up Effect")]
    public class KnockUpEffect : InstantCharacterEffect
    {
        public float liftHeight;
        public float floatDuration;
        public float slamDamage;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            character.aiCharacterManager?.KnockUp(liftHeight, floatDuration, slamDamage);
        }

    }
}

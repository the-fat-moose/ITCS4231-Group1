using System.Collections;
using System.Collections.Generic;
using UnityEngine;      

namespace Group1
{    
    [CreateAssetMenu(menuName = "Effects/Instant Effects/Freeze Effect")]
    public class FreezeEffect : InstantCharacterEffect
    {
        public float duration;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            // Call AI freeze logic
            character.aiCharacterManager?.Freeze(duration);
        }

    }
}

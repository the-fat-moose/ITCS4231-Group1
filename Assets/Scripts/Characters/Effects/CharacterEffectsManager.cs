using UnityEngine;

namespace Group1 {
    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        // PROCESS INSTANT EFFECTS (TAKE DAMAGE, HEAL)
        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            // TAKE IN AN EFFECT & PROCESS IT
            if (character != null) 
            {
                effect.ProcessEffect(character);
            }
        }

        // PROCESS TIMED EFFECTS (POISON, BUILD UPS)

        // PROCESS STATIC EFFECTS (ADDING/REMOVING BUFFS FROM LUMENS ETC)
    }
}

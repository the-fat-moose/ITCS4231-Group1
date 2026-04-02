using UnityEngine;

namespace Group1 {
    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Current Active FX")]
        public GameObject activeQuickSlotItemFX;

        [Header("VFX")]
        [SerializeField] GameObject bloodSplatterVFX;

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

        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            // IF WE MANUALLY HAVE PLACED A BLOOD SPLATTER VFX ON THIS MODEL, PLAY ITS VERSION
            if (bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            // ELSE USE THE GENERIC VERSION
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
        }
    }
}

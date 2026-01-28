using System.Runtime.CompilerServices;
using UnityEngine;

namespace Group1 {
    [CreateAssetMenu(menuName = "Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage; // If the damage is caused by another character's attack, it will be stored here

        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;

        [Header("Final Damage")]
        private int finalDamage = 0; // damage a character takes after all damages are calculated together, accounting for resistances and modifiers

        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX; // USED ON TOP OF REGULAR SOUND EFFECT IF THERE IS ELEMENTAL DAMAGE PRESENT (Magic)

        [Header("Direction Damage Taken From")]
        public float angleHitFrom; // USED TO DETERMINE WHAT DAMAGE ANIMATION TO PLAY (Move backwards, left, right, etc)
        public Vector3 contactPoint; // USED TO DETERMINE WHERE THE BLOOD FX INSTANTIATES

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            // IF THE CHARACTER IS DEAD, NO ADDITIONAL DAMAGE EFFECTS SHOULD BE PROCESSED
            if (character.isDead) return;

            // CHECK FOR INVULNERABILITY

            CalculateDamage(character);
            // CHECK WHICH DIRECTION THE DAMAGE CAME FROM
            // PLAY A DAMAGE ANIMATION (IF APPLICABLE)
            // PLAY DAMAGE SOUND FX
            // PLAY DAMAGE VFX (BLOOD)

            // IF CHARACTER IS A.I, CHECK FOR NEW TARGET IF CHARACTER CAUSING DAMAGE IS PRESENT
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (characterCausingDamage != null)
            {
                // CHECK FOR DAMAGE MODIFIERS AND MODIFY BASE DAMAGE (Physical damage buff, magic damage buff, etc)
            }

            // CHECK CHARACTER FOR FLAT DAMAGE REDUCTION AND SUBTRACT THEM FROM THE DAMAGE

            // ADD ALL DAMAGE TYPES TOGETHER AND PROCESS AND APPLY FINAL DAMAGE
            finalDamage = Mathf.RoundToInt(physicalDamage + magicDamage);

            if (finalDamage <= 0)
            {
                finalDamage = 1;
            }

            character.CurrentHealth -= finalDamage;
        }
    }
}
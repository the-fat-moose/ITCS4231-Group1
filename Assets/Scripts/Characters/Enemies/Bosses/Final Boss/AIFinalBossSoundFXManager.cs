using UnityEngine;

namespace Group1
{
    public class AIFinalBossSoundFXManager : CharacterSoundFXManager
    {
        [Header("Sword Whooshes")]
        public AudioClip[] finalBossSwordWhooshes;

        [Header("AOE Impacts")]
        public AudioClip[] aoeImpacts;

        public virtual void PlayAOEImpactSoundFX()
        {
            if (aoeImpacts.Length > 0)
            {
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(aoeImpacts));
            }
        }
    }
}
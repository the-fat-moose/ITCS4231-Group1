using UnityEngine;

namespace Group1 {
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        [Header("Damage Grunts")]
        [SerializeField] protected AudioClip[] damageGrunts;

        [Header("Attack Grunts")]
        [SerializeField] protected AudioClip[] attackGrunts;

        [Header("Footsteps")]
        [SerializeField] protected AudioClip[] footSteps;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySoundFX(AudioClip soundFX, float volume = 1f, bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            audioSource.PlayOneShot(soundFX, volume);
            // RESETS OUR PITCH
            audioSource.pitch = 1;
            
            if (randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }
        }

        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX, 0.25f);
        }

        public void PlayDamageGruntSoundFX()
        {
            if (damageGrunts.Length > 0)
            {
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(damageGrunts));
            }
        }

        public void PlayAttackGruntSoundFX()
        {
            if (attackGrunts.Length > 0)
            {
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackGrunts));
            }
        }

        public void PlayFootStepSoundFX()
        {
            if (footSteps.Length > 0)
            {
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(footSteps));
            }
        }
    }
}
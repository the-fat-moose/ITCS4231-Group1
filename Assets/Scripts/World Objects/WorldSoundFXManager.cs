using System.Collections;
using UnityEngine;

namespace Group1 {
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;

        [Header("Boss Track")]
        [SerializeField] AudioSource bossIntroPlayer;
        [SerializeField] AudioSource bossLoopPlayer;

        [Header("Damage Sounds")]
        public AudioClip[] physicalDamageSFX;
        public AudioClip[] blockSFX;
        public AudioClip[] parrySFX;

        [Header("Action Sounds")]
        public AudioClip pickupItemSFX;
        public AudioClip rollSFX;
        public AudioClip healingSFX;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void PlayBossTrack(AudioClip introTrack, AudioClip loopTrack)
        {
            bossIntroPlayer.volume = 1;
            bossLoopPlayer.volume = 1;
            if (introTrack != null)
            {
                bossIntroPlayer.clip = introTrack;
                bossIntroPlayer.loop = false;
                bossIntroPlayer.Play();
                bossLoopPlayer.clip = loopTrack;
                bossLoopPlayer.loop = true;
                bossLoopPlayer.PlayDelayed(bossIntroPlayer.clip.length);
            }
            else
            {
                bossLoopPlayer.clip = loopTrack;
                bossLoopPlayer.loop = true;
                bossLoopPlayer.Play();
            }
        }
    
        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            int index = Random.Range(0, array.Length);

            return array[index];
        }

        public void StopBossMusic()
        {
            StartCoroutine(FadeOutBossMusicThenStop());
        }

        private IEnumerator FadeOutBossMusicThenStop()
        {
            

            while (bossLoopPlayer.volume > 0)
            {
                bossLoopPlayer.volume -= Time.deltaTime;
                bossIntroPlayer.volume -= Time.deltaTime;
                yield return null;
            }

            bossIntroPlayer.Stop();
            bossLoopPlayer.Stop();
        }

        public void StopAllAudio()
        {
            StopBossMusic();
            // ANY OTHER AUDIOS THAT NEED TO BE STOPPED
        }
    }
}
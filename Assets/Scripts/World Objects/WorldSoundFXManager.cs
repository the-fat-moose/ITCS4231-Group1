using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Group1 {
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;

        [Header("Universal Music Volume")]
        [SerializeField] private float musicVolume = 0.125f;

        [Header("Boss Track")]
        [SerializeField] AudioSource bossIntroPlayer;
        [SerializeField] AudioSource bossLoopPlayer;

        [Header("Level Tracks")]
        [SerializeField] List<AudioClip> levelTracks = new List<AudioClip>();
        [SerializeField] AudioSource levelMusicPlayer;

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
            StopLevelMusic();

            bossIntroPlayer.volume = musicVolume;
            bossLoopPlayer.volume = musicVolume;
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

        public void PlayLevelTrack(int sceneIndex)
        {
            StopAllAudio();

            if (levelMusicPlayer != null && levelMusicPlayer.isPlaying)
            {
                StartCoroutine(FadeInNewLevelTrack(sceneIndex));
            }
            else if (levelMusicPlayer != null && levelTracks[sceneIndex] != null)
            {
                levelMusicPlayer.volume = musicVolume;
                levelMusicPlayer.clip = levelTracks[sceneIndex];
                levelMusicPlayer.loop = true;
                levelMusicPlayer.Play();
            }
        }

        private IEnumerator FadeInNewLevelTrack(int sceneIndex)
        {
            StartCoroutine(FadeOutLevelMusicThenStop());

            while (levelMusicPlayer.isPlaying) yield return null;

            if (levelTracks[sceneIndex] != null)
            {
                levelMusicPlayer.clip = levelTracks[sceneIndex];
                levelMusicPlayer.volume = 0;
                levelMusicPlayer.loop = true;
                levelMusicPlayer.Play();

                while (levelMusicPlayer.volume < musicVolume)
                {
                    levelMusicPlayer.volume += 5 * Time.deltaTime;
                    yield return null;
                }

                levelMusicPlayer.volume = musicVolume;
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

        public void StopBossMusicInstant()
        {
            bossIntroPlayer.volume = 0;
            bossLoopPlayer.volume = 0;

            bossIntroPlayer.Stop();
            bossLoopPlayer.Stop();
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

        public void StopLevelMusic()
        {
            StartCoroutine(FadeOutLevelMusicThenStop());
        }

        public void StopLevelMusicInstant()
        {
            levelMusicPlayer.volume = 0;
            levelMusicPlayer.Stop();
        }

        private IEnumerator FadeOutLevelMusicThenStop()
        {
            while (levelMusicPlayer.volume > 0)
            {
                levelMusicPlayer.volume -= 5 * Time.deltaTime;
                yield return null;
            }

            levelMusicPlayer.Stop();
        }

        public void StopAllAudio()
        {
            StopBossMusicInstant();
            StopLevelMusicInstant();
            // ANY OTHER AUDIOS THAT NEED TO BE STOPPED
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Group1
{
    public class WorldGameSessionManager : MonoBehaviour
    {
        public static WorldGameSessionManager instance;

        private Coroutine revivalCoroutine;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;

            WorldSoundFXManager.instance.StopAllAudio();
            WorldSoundFXManager.instance.PlayLevelTrack(SceneManager.GetActiveScene().buildIndex);

            PlayerUIManager.instance.playerUIHudManager.ToggleHUD(true);

            // If title screen, unlock cursor, disable player hud
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                PlayerUIManager.instance.playerUIHudManager.ToggleHUD(false);
            }
        }

        private void OnSceneChanged(Scene arg0, Scene arg1)
        {
            StartCoroutine(HandleSceneLoaded());
        }

        private IEnumerator HandleSceneLoaded()
        {
            // Stop Preexisting played music
            WorldSoundFXManager.instance.StopAllAudio();

            LockCursor();
            PlayerUIManager.instance.playerUIHudManager.ToggleHUD(true);

            // If title screen, unlock cursor, disable player hud
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                UnlockCursor();
                PlayerUIManager.instance.playerUIHudManager.ToggleHUD(false);
            }

            // Play Music
            WorldSoundFXManager.instance.PlayLevelTrack(SceneManager.GetActiveScene().buildIndex);

            // Wait one frame so geodes can register
            yield return null;

            PlayerManager player = FindFirstObjectByType<PlayerManager>();
            if (player == null)
                yield break;

            CharacterSaveData data = WorldSaveGameManager.instance.currentCharacterData;

            // Try to find the geode the player last rested at
            GeodeInteractable target = WorldObjectManager.instance.geodes
                .Find(g => g.geodeID == data.lastGeodeRestedAt &&
                        g.sceneIndex == SceneManager.GetActiveScene().buildIndex);

            if (target != null)
            {
                Vector3 pos = target.teleportTransform.position;
                pos.y += 2f; // small lift to avoid clipping
                player.transform.position = pos;
            }
            else
            {
                // Fallback to safe teleport
                SafeTeleportPosition tp = FindFirstObjectByType<SafeTeleportPosition>();
                if (tp != null)
                {
                    Vector3 pos = tp.transform.position;
                    pos.y += 2f;
                    player.transform.position = pos;
                }
            }
        }

        public void WaitThenRevivePlayer()
        {
            if (revivalCoroutine != null)
                StopCoroutine(revivalCoroutine);

            revivalCoroutine = StartCoroutine(RevivePlayerCoroutine(5));
        }

        private IEnumerator RevivePlayerCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            PlayerUIManager.instance.playerUIHudManager.RemoveBossHealthBars();
            PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();

            PlayerManager player = FindFirstObjectByType<PlayerManager>();

            if (player != null)
            player.ReviveCharacter();

            for (int i = 0; i < WorldObjectManager.instance.geodes.Count; i++)
            {
                if (WorldObjectManager.instance.geodes[i].geodeID == WorldSaveGameManager.instance.currentCharacterData.lastGeodeRestedAt)
                {
                    WorldObjectManager.instance.geodes[i].TeleportToGeode();
                    break;
                }
            }

            // RESPAWN ALL ENEMIES
            WorldAIManager.instance.RespawnAllCharacters();

            // Play Music
            WorldSoundFXManager.instance.PlayLevelTrack(SceneManager.GetActiveScene().buildIndex);
        }
    
        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
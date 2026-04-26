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
        }

        private void OnSceneChanged(Scene arg0, Scene arg1)
        {
            // Find safe place to put player
            PlayerManager player = FindFirstObjectByType<PlayerManager>();

            if (player != null) 
            {
                player.FindSafePlaceForPlayer();

                WorldSaveGameManager.instance.SaveGame();
                LockCursor();

                // if the scene is the main menu unlock cursor
                if (SceneManager.GetActiveScene().buildIndex == 0) UnlockCursor();
            }

            WorldSoundFXManager.instance.StopBossMusic();
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Destroy(this);
            }
            DontDestroyOnLoad(this);
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
    }
}
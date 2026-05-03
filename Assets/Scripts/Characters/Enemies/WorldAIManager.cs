using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Group1 {
    public class WorldAIManager : MonoBehaviour
    {
        public static WorldAIManager instance;

        [Header("Loading")]
        public bool isPerformingLoadingOperation = false;

        [Header("Characters")]
        [SerializeField] private List<AICharacterSpawner> aiCharacterSpawners;
        public List<AICharacterManager> spawnedInCharacters;
        private Coroutine spawnAllCharactersCoroutine;
        private Coroutine respawnAllCharactersCoroutine;
        private Coroutine despawnAllCharactersCoroutine;
        private Coroutine resetAllCharactersCoroutine;

        [Header("Bosses")]
        public List<AIBossCharacterManager> spawnedInBossCharacters;

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
        
        public void SpawnCharacter(AICharacterSpawner spawner)
        {
            aiCharacterSpawners.Add(spawner);
            StartCoroutine(spawner.AttemptToSpawnCharacter());
        }

        public void AddCharacterToSpawnedCharactersList(AICharacterManager character)
        {
            if (spawnedInCharacters.Contains(character)) return;

            spawnedInCharacters.Add(character);

            AIBossCharacterManager bossCharacter = character as AIBossCharacterManager;

            if (bossCharacter != null)
            {
                if (spawnedInBossCharacters.Contains(bossCharacter)) return;

                spawnedInBossCharacters.Add(bossCharacter);
            }
        }

        public AIBossCharacterManager GetBossCharacterByID(int ID)
        {
            return spawnedInBossCharacters.FirstOrDefault(boss => boss.bossID == ID);
        }

        // Initial Spawning of Characters
        public void SpawnAllCharacters()
        {
            isPerformingLoadingOperation = true;

            if (spawnAllCharactersCoroutine != null) StopCoroutine(spawnAllCharactersCoroutine);

            spawnAllCharactersCoroutine = StartCoroutine(SpawnAllCharactersCoroutine());
        }

        private IEnumerator SpawnAllCharactersCoroutine()
        {
            for (int i = 0; i < aiCharacterSpawners.Count; i++)
            {
                yield return new WaitForFixedUpdate();

                aiCharacterSpawners[i].AttemptToSpawnCharacter();

                yield return null;
            }

            isPerformingLoadingOperation = false;

            yield return null;
        }

        // Respawn all Characters (could be taxing with a lot of enemies)
        public void RespawnAllCharacters()
        {
            isPerformingLoadingOperation = true;

            if (respawnAllCharactersCoroutine != null) StopCoroutine(respawnAllCharactersCoroutine);

            respawnAllCharactersCoroutine = StartCoroutine(RespawnAllCharactersCoroutine());
        }

        private IEnumerator RespawnAllCharactersCoroutine()
        {
            // DESPAWN ALL CHARACTERS
            for (int i = 0; i < spawnedInCharacters.Count; i++)
            {
                yield return new WaitForFixedUpdate();

                Destroy(spawnedInCharacters[i].gameObject);

                yield return null;
            }

            spawnedInCharacters.Clear();
            spawnedInBossCharacters.Clear();

            // SPAWN THE CHARACTERS BACK IN
            for (int i = 0; i < aiCharacterSpawners.Count; i++)
            {
                yield return new WaitForFixedUpdate();

                StartCoroutine(aiCharacterSpawners[i].AttemptToSpawnCharacter());

                yield return null;
            }

            isPerformingLoadingOperation = false;

            yield return null;
        }

        // Reset Spawned in Characters (can be used instead of RespawnAllCharacters to reduce instantiation load)
        public void ResetAllCharacters()
        {
            isPerformingLoadingOperation = true;

            if (resetAllCharactersCoroutine != null) StopCoroutine(resetAllCharactersCoroutine);

            resetAllCharactersCoroutine = StartCoroutine(ResetAllCharactersCoroutine());
        }

        private IEnumerator ResetAllCharactersCoroutine()
        {
            for (int i = 0; i < aiCharacterSpawners.Count; i++)
            {
                yield return new WaitForFixedUpdate();

                aiCharacterSpawners[i].ResetCharacter();

                yield return null;
            }

            isPerformingLoadingOperation = false;

            yield return null;
        }

        // Despawning of Characters
        private void DespawnAllCharacters()
        {
            isPerformingLoadingOperation = true;

            if (despawnAllCharactersCoroutine != null) StopCoroutine(despawnAllCharactersCoroutine);

            despawnAllCharactersCoroutine = StartCoroutine(DespawnAllCharactersCoroutine());
        }

        private IEnumerator DespawnAllCharactersCoroutine()
        {
            for (int i = 0; i < spawnedInCharacters.Count; i++)
            {
                yield return new WaitForFixedUpdate();

                Destroy(spawnedInCharacters[i].gameObject);

                yield return null;
            }

            spawnedInCharacters.Clear();
            isPerformingLoadingOperation = false;

            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Group1 {
    public class WorldAIManager : MonoBehaviour
    {
        public static WorldAIManager instance;

        [Header("Characters")]
        [SerializeField] private List<AICharacterSpawner> aiCharacterSpawners;
        public List<AICharacterManager> spawnedInCharacters;

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
            spawner.AttemptToSpawnCharacter();
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

        public void ResetAllCharacters()
        {
            DespawnAllCharacters();

            foreach (var spawner in aiCharacterSpawners)
            {
                spawner.AttemptToSpawnCharacter();
            }
        }

        private void DespawnAllCharacters()
        {
            foreach (var character in spawnedInCharacters)
            {
                Destroy(character);
            }

            spawnedInCharacters.Clear();
        }

        private void DisableAllCharacters()
        {
            
        }
    }
}

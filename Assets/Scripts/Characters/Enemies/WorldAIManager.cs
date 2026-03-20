using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Group1 {
    public class WorldAIManager : MonoBehaviour
    {
        public static WorldAIManager instance;

        [Header("Characters")]
        [SerializeField] private List<AICharacterSpawner> aiCharacterSpawners;
        public List<GameObject> spawnedInCharacters;

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

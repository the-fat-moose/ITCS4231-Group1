using UnityEngine;

namespace Group1 {
    public class AICharacterSpawner : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] GameObject characterGameObject;
        [SerializeField] GameObject instantiatedGameObject;
        private AICharacterManager aiCharacter;

        private void Awake()
        {
            
        }

        private void Start()
        {
            WorldAIManager.instance.SpawnCharacter(this);
            gameObject.SetActive(false);
        }

        public void AttemptToSpawnCharacter()
        {
            if (characterGameObject != null)
            {
                instantiatedGameObject = Instantiate(characterGameObject);
                instantiatedGameObject.transform.position = transform.position;
                instantiatedGameObject.transform.rotation = transform.rotation;
                aiCharacter = instantiatedGameObject.GetComponent<AICharacterManager>();

                if (aiCharacter != null)
                    WorldAIManager.instance.AddCharacterToSpawnedCharactersList(aiCharacter);
            }
        }

        public void ResetCharacter()
        {
            if (instantiatedGameObject == null) return;

            if (aiCharacter == null) return;

            // RESET POSITION AND HEALTH
            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            aiCharacter.CurrentHealth = aiCharacter.MaxHealth;
                
            if (aiCharacter.isDead)
            {
                aiCharacter.isDead = false;
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Empty", false, false, true, true, true, true);
            }

            // RESET ENEMY STATE
            aiCharacter.characterCombatManager.currentTarget = null;
            aiCharacter.currentState = aiCharacter.idle;

            aiCharacter.characterUIManager.ResetCharacterHPBar();
        }
    }
}
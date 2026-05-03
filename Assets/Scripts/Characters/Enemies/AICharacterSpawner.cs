using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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

        public IEnumerator AttemptToSpawnCharacter()
        {
            if (characterGameObject != null)
            {
                instantiatedGameObject = Instantiate(characterGameObject);
                instantiatedGameObject.transform.position = transform.position;
                instantiatedGameObject.transform.rotation = transform.rotation;
                aiCharacter = instantiatedGameObject.GetComponent<AICharacterManager>();

                if (aiCharacter != null)
                {
                    WorldAIManager.instance.AddCharacterToSpawnedCharactersList(aiCharacter);

                    while (!NavMesh.SamplePosition(instantiatedGameObject.transform.position, out _, 1f, NavMesh.AllAreas))
                        yield return null;

                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(instantiatedGameObject.transform.position, out hit, 5f, NavMesh.AllAreas))
                    {
                        instantiatedGameObject.transform.position = hit.position;
                        instantiatedGameObject.GetComponentInChildren<NavMeshAgent>().Warp(hit.position);
                    }
                }
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
            aiCharacter.characterCombatManager.SetTarget(null);
            aiCharacter.currentState = aiCharacter.idle;

            aiCharacter.characterUIManager.ResetCharacterHPBar();
        }
    }
}
using UnityEngine;

namespace Group1 {
    public class WorldObjectSpawner : MonoBehaviour
    {
        [Header("Object")]
        [SerializeField] GameObject worldGameObject;
        [SerializeField] GameObject instantiatedGameObject;

        private void Awake()
        {
            
        }

        private void Start()
        {
            WorldObjectManager.instance.SpawnObject(this);
            gameObject.SetActive(false);
        }

        public void AttemptToSpawnObject()
        {
            if (worldGameObject != null)
            {
                instantiatedGameObject = Instantiate(worldGameObject);
                instantiatedGameObject.transform.position = transform.position;
                instantiatedGameObject.transform.rotation = transform.rotation;
            }
        }
    }
}
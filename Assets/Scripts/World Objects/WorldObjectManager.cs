using System.Collections.Generic;
using UnityEngine;

namespace Group1 {
    public class WorldObjectManager : MonoBehaviour
    {
        public static WorldObjectManager instance;

        [Header("World Objects")]
        [SerializeField] private List<WorldObjectSpawner> worldObjectSpawners;
        [SerializeField] private List<GameObject> spawnedInObjects;

        [Header("Fog Walls")]
        public List<FogWallInteractable> fogWalls;

        [Header("Geode Locations")]
        public List<GeodeInteractable> geodes;

        // Spawn in those fog walls during start of game (must have a spawner object)
        // Create a general object spawner script and prefab
        // When the fog walls are spawned, add them to the world fog wall list
        // Grab the correct fogwall from the list on the boss manager when the boss is being initialized

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

        public void SpawnObject(WorldObjectSpawner spawner)
        {
            worldObjectSpawners.Add(spawner);
            spawner.AttemptToSpawnObject();
        }

        public void AddFogWallToList(FogWallInteractable fogWall)
        {
            if (!fogWalls.Contains(fogWall))
            {
                fogWalls.Add(fogWall);
            }
        }

        public void RemoveFogWallFromList(FogWallInteractable fogWall)
        {
            if (fogWalls.Contains(fogWall))
            {
                fogWalls.Remove(fogWall);
            }
        }

        public void AddGeodeToList(GeodeInteractable geode)
        {
            if (!geodes.Contains(geode))
            {
                geodes.Add(geode);
            }
        }

        public void RemoveGeodeFromList(GeodeInteractable geode)
        {
            if (geodes.Contains(geode))
            {
                geodes.Remove(geode);
            }
        }
    }
}
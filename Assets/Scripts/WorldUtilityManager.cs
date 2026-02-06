using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

namespace Group1
{
    public class WorldUtilityManager : MonoBehaviour
    {
        public WorldUtilityManager Instance;

        [Header("Layers")]
        [SerializeField] LayerMask characterLayers;
        [SerializeField] LayerMask enviroLayers;

        private void Awake()
        {
            if(Instance == null) Instance = this;
            else Destroy(gameObject);

        }
    
        public LayerMask GetCharacterLayers()
        {
            return characterLayers;
        }

        public LayerMask GetEnviroLayers()
        {
            return enviroLayers;
        }


    }   
}
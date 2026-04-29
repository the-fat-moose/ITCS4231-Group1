using UnityEngine;
using UnityEngine.SceneManagement;

namespace Group1
{
    public class SceneTeleporter : MonoBehaviour
    {
        [Header("Scene")]
        [SerializeField] private int buildIndexToTravelTo = 1;

        void OnTriggerEnter(Collider other)
        {
            if (buildIndexToTravelTo <= 1 || buildIndexToTravelTo >= SceneManager.sceneCountInBuildSettings)
            {
                WorldSaveGameManager.instance.SaveAndQuit();
            }
            else
            {
                WorldSaveGameManager.instance.LoadNewScene(buildIndexToTravelTo);
            }
        }
    }
}
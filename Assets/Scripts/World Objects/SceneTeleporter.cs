using UnityEngine;
using UnityEngine.SceneManagement;

namespace Group1
{
    public class SceneTeleporter : MonoBehaviour
    {
        [Header("Status")]
        [SerializeField] private bool isReturnTeleporter = false;

        void OnTriggerEnter(Collider other)
        {
            int buildIndex = SceneManager.GetActiveScene().buildIndex;

            if (isReturnTeleporter) 
            {
                buildIndex -= 1;
            }
            else 
            {
                buildIndex += 1;
            }

            if (buildIndex <= 1 || buildIndex >= SceneManager.sceneCountInBuildSettings)
            {
                // Out of range, fall back to main menu
                buildIndex = 1;
                WorldSaveGameManager.instance.SaveAndQuit();
            }
            WorldSaveGameManager.instance.LoadNewScene(buildIndex);
        }
    }
}
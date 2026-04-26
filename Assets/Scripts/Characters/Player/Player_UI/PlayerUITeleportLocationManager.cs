using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Group1
{
    public class PlayerUITeleportLocationManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject menu;

        [SerializeField] GameObject[] teleportLocations;

        public void OpenTeleportLocationMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = true;
            menu.SetActive(true);

            CheckForUnlockedTeleports();
        }

        public void CloseTeleportLocationMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

        private void CheckForUnlockedTeleports()
        {
            bool hasFirstSelectedButton = false;
            for (int i = 0; i < teleportLocations.Length; i++)
            {
                for (int j = 0; j < WorldObjectManager.instance.geodes.Count; j++)
                {
                    if (WorldObjectManager.instance.geodes[j].geodeID == i)
                    {
                        if (WorldObjectManager.instance.geodes[j].IsActivated && SceneManager.GetActiveScene().buildIndex == WorldObjectManager.instance.geodes[j].sceneIndex)
                        {
                            teleportLocations[i].SetActive(true);

                            if (!hasFirstSelectedButton)
                            {
                                hasFirstSelectedButton = true;
                                teleportLocations[i].GetComponent<Button>().Select();
                                teleportLocations[i].GetComponent<Button>().OnSelect(null);
                            }
                        }
                        else
                        {
                            teleportLocations[i].SetActive(false);
                        }
                    }
                }
            }
        }

        public void TeleportToGeode(int geodeID)
        {
            for (int i = 0; i < WorldObjectManager.instance.geodes.Count; i++)
            {
                if (WorldObjectManager.instance.geodes[i].geodeID == geodeID && SceneManager.GetActiveScene().buildIndex == WorldObjectManager.instance.geodes[i].sceneIndex)
                {
                    // TELEPORT
                    WorldObjectManager.instance.geodes[i].TeleportToGeode();

                    return;
                }
            }
        }
    }
}
using System.Collections;
using UnityEngine;

namespace Group1
{
    public class PlayerUICharacterMenuManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] private GameObject menu;

        public void OpenCharacterMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = true;
            menu.SetActive(true);
        }

        public void CloseCharacterMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

        public void CloseCharacterMenuAfterFixedUpdate()
        {
            StartCoroutine(WaitThenCloseMenu());

            WorldGameSessionManager.instance.LockCursor();
        }

        private IEnumerator WaitThenCloseMenu()
        {
            yield return new WaitForFixedUpdate();

            CloseCharacterMenu();
        }

        public void SaveAndQuit()
        {
            CloseCharacterMenu();

            WorldSaveGameManager.instance.SaveAndQuit();
        }
    }
}
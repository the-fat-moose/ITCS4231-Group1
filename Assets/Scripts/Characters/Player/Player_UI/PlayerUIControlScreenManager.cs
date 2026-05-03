using UnityEngine;

namespace Group1
{
    public class PlayerUIControlScreenManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject menu;

        public void OpenControlScreenMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = true;
            menu.SetActive(true);
        }

        public void CloseControlScreenMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = false;
            menu.SetActive(false);

            WorldGameSessionManager.instance.LockCursor();
        }
    }
}
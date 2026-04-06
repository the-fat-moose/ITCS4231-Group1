using UnityEngine;

namespace Group1
{
    public class PlayerUIGeodeManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject menu;

        public void OpenGeodeMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = true;
            menu.SetActive(true);
        }

        public void CloseGeodeMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }
    
        public void OpenTeleportLocationMenu()
        {
            CloseGeodeMenu();
            PlayerUIManager.instance.playerUITeleportLocationManager.OpenTeleportLocationMenu();
        }
    }
}
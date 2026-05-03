using UnityEngine;

namespace Group1 {
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;

        [HideInInspector] public PlayerUIHudManager playerUIHudManager;
        [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;
        [HideInInspector] public PlayerUICharacterMenuManager playerUICharacterMenuManager;
        [HideInInspector] public PlayerUIEquipmentManager playerUIEquipmentManager;
        [HideInInspector] public PlayerUIGeodeManager playerUIGeodeManager;
        [HideInInspector] public PlayerUITeleportLocationManager playerUITeleportLocationManager;
        [HideInInspector] public PlayerUILoadingScreenManager playerUILoadingScreenManager;
        [HideInInspector] public PlayerUIControlScreenManager playerUIControlScreenManager;

        [Header("UI Flags")]
        public bool menuWindowIsOpen = false; // INVENTORY SCREEN, EQUIPMENT MENU, ETC
        public bool popUpWindowIsOpen = false; // ITEM PICK UP

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

            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
            playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
            playerUICharacterMenuManager = GetComponentInChildren<PlayerUICharacterMenuManager>();
            playerUIEquipmentManager = GetComponentInChildren<PlayerUIEquipmentManager>();
            playerUIGeodeManager = GetComponentInChildren<PlayerUIGeodeManager>();
            playerUITeleportLocationManager = GetComponentInChildren<PlayerUITeleportLocationManager>();
            playerUILoadingScreenManager = GetComponentInChildren<PlayerUILoadingScreenManager>();
            playerUIControlScreenManager = GetComponentInChildren<PlayerUIControlScreenManager>();
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        public void CloseAllMenuWindows()
        {
            playerUICharacterMenuManager.CloseCharacterMenu();
            playerUIEquipmentManager.CloseEquipmentMenu();
            playerUIGeodeManager.CloseGeodeMenu();
            playerUITeleportLocationManager.CloseTeleportLocationMenu();
            playerUIControlScreenManager.CloseControlScreenMenu();
        }
    }
}
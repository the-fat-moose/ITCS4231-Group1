using UnityEngine;

namespace Group1 {
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;

        [HideInInspector] public PlayerUIHudManager playerUIHudManager;
        [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;

        private void Awake()
        {
            Debug.LogError("PlayerUIManager AWAKE");

            if (instance == null)
            {
                Debug.LogError("PlayerUIManager Instance was null");

                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Debug.LogError("PlayerUIManager Instance wasn't null");

                Destroy(gameObject);
            }

            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
            playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();

            Debug.LogError("playerUIHudManager: " + playerUIHudManager);
            Debug.LogError("playerUIPopUpManager: " + playerUIPopUpManager);
        }
    }
}

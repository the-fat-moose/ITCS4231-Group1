using UnityEngine;

namespace Group1
{
    public class PlayerUIToggleHud : MonoBehaviour
    {
        void OnEnable()
        {
            // HIDE THE HUD
            PlayerUIManager.instance.playerUIHudManager.ToggleHUD(false);
        }

        void OnDisable()
        {
            // BRING THE HUD BACK
            PlayerUIManager.instance.playerUIHudManager.ToggleHUD(true);
        }
    }
}
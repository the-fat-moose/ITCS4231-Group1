using UnityEngine;

namespace Group1 {
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNewGame()
        {
            WorldSaveGameManager.instance.CreateNewGame();
            StartCoroutine(WorldSaveGameManager.instance.LoadWorldScene());
        }
    }
}
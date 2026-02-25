using UnityEngine;

namespace Group1 {
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNewGame()
        {
            StartCoroutine(WorldSaveGameManager.instance.LoadNewGame());
        }
    }
}
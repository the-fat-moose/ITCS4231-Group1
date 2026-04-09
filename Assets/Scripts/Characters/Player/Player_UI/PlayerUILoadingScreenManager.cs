using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Group1
{
    public class PlayerUILoadingScreenManager : MonoBehaviour
    {
        [SerializeField] GameObject loadingScreen;
        [SerializeField] CanvasGroup canvasGroup;
        private Coroutine fadeLoadingScreenCoroutine;

        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnSceneChanged(Scene arg0, Scene arg1)
        {
            DeactivateLoadingScreen();
        }

        public void ActivateLoadingScreen()
        {
            if (loadingScreen.activeSelf) return;

            canvasGroup.alpha = 1f;
            loadingScreen.SetActive(true);
        }

        public void DeactivateLoadingScreen(float delay = 1f)
        {
            if (!loadingScreen.activeSelf) return;

            // IF WE ARE ALREADY FADING AWAY THE LOADING SCREEN, RETURN
            if (fadeLoadingScreenCoroutine != null) return;

            fadeLoadingScreenCoroutine = StartCoroutine(FadeLoadingScreen(1f, delay));
        }

        private IEnumerator FadeLoadingScreen(float duration, float delay)
        {
            while (WorldAIManager.instance.isPerformingLoadingOperation)


            loadingScreen.SetActive(true);

            if (duration > 0)
            {
                while (delay > 0)
                {
                    delay -= Time.deltaTime;
                    yield return null;
                }

                canvasGroup.alpha = 1;
                float elaspedTime = 0;

                while (elaspedTime < duration)
                {
                    elaspedTime += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(1, 0, elaspedTime / duration);
                    yield return null;
                }
            }

            canvasGroup.alpha = 0;
            loadingScreen.SetActive(false);
            fadeLoadingScreenCoroutine = null;

            yield return null;
        }
    }
}
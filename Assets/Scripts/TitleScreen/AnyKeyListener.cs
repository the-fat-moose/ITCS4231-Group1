using UnityEngine;
using UnityEngine.SceneManagement;

namespace Group1
{
    public class AnyKeyListener : MonoBehaviour
    {
        [SerializeField] GameObject confirmationText;
        private bool showsConfirmationText = false;

        void Awake()
        {
            confirmationText.SetActive(false);
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                OnAnyButton();
            }
        }

        private void OnAnyButton()
        {
            if (!showsConfirmationText)
            {
                showsConfirmationText = true;
                confirmationText.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene(1); // MAIN MENU
            }
        }
    }
}
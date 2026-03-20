using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

namespace Group1 {
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("Message Pop Up")]
        [SerializeField] private TextMeshProUGUI popUpMessageText;
        [SerializeField] private GameObject popUpMessageGameObject;

        [Header("Item Pop Up")]
        [SerializeField] private GameObject itemPopUpGameObject;
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemAmount;

        [Header("GEODE SHATTERED Pop Up")]
        [SerializeField] private GameObject geodeShatteredPopUpGameObject;
        [SerializeField] private TextMeshProUGUI geodeShatteredPopUpBackgroundText;
        [SerializeField] private TextMeshProUGUI geodeShatteredPopUpText;
        [SerializeField] private CanvasGroup geodeShatteredPopUpCanvasGroup;

        [Header("YOU DIED Pop Up")]
        [SerializeField] private GameObject youDiedPopUpGameObject;
        [SerializeField] private TextMeshProUGUI youDiedPopUpBackgroundText;
        [SerializeField] private TextMeshProUGUI youDiedPopUpText;
        [SerializeField] private CanvasGroup youDiedPopUpCanvasGroup; // allows us to set the alpha to fade over time

        public void CloseAllPopUpWindows()
        {
            popUpMessageGameObject.SetActive(false);
            itemPopUpGameObject.SetActive(false);

            PlayerUIManager.instance.popUpWindowIsOpen = false;
        }

        public void SendPlayerMessagePopUp(string messageText)
        {
            PlayerUIManager.instance.popUpWindowIsOpen = true;
            popUpMessageText.text = messageText;
            popUpMessageGameObject.SetActive(true);
        }

        public void SendItemPopUp(Item item, int amount)
        {
            itemAmount.enabled = false;
            itemIcon.sprite = item.itemIcon;
            itemName.text = item.itemName;

            if (amount > 1)
            {
                itemAmount.enabled = true;
                itemAmount.text = "x" + amount.ToString();
            }

            itemPopUpGameObject.SetActive(true);
            PlayerUIManager.instance.popUpWindowIsOpen = true;
        }        

        public void SendGeodeShatteredPopUp(string geodeShatteredMessage)
        {
            // ACTIVATE ANY POST PROCESSING EFFECTS

            geodeShatteredPopUpText.text = geodeShatteredMessage;
            geodeShatteredPopUpBackgroundText.text = geodeShatteredMessage;
            geodeShatteredPopUpGameObject.SetActive(true);
            geodeShatteredPopUpBackgroundText.characterSpacing = 0;
            StartCoroutine(StretchPopUpTextOverTime(geodeShatteredPopUpBackgroundText, 8f, 19f));
            StartCoroutine(FadeInPopUpOverTime(geodeShatteredPopUpCanvasGroup, 5f));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(geodeShatteredPopUpCanvasGroup, 2f, 5f));
        }
        
        public void SendYouDiedPopUp()
        {
            // ACTIVATE ANY POST PROCESSING EFFECTS

            youDiedPopUpGameObject.SetActive(true);
            youDiedPopUpBackgroundText.characterSpacing = 0;
            StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8f, 19f));
            StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5f));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2f, 5f));
        }

        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if (duration > 0f)
            {
                text.characterSpacing = 0; // RESETS OUR CHARACTER SPACING
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));

                    yield return null;
                }
            }
        }

        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
        {
            if (duration > 0)
            {
                canvas.alpha = 0;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);

                    yield return null;
                }
            }

            canvas.alpha = 1;

            yield return null;
        }

        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
        {
            if (duration > 0)
            {
                while (delay > 0)
                {
                    delay = delay - Time.deltaTime;
                    yield return null;
                }

                canvas.alpha = 1;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);

                    yield return null;
                }
            }

            canvas.alpha = 0;

            yield return null;
        }
    }
}
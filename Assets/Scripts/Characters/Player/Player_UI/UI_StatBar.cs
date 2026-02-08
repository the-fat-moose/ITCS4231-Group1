using UnityEngine;
using UnityEngine.UI;

namespace Group1 {
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;
        private RectTransform rectTransform;
        
        [Header("Bar Options")]
        [SerializeField] protected bool scaleBarLengthWithStats = true;
        [SerializeField] protected float widthScaleMultiplier = 1f;
        // SECONDARY BAR BEHIND MAIN BAR FOR POLISH EFFECT (YELLOW BAR THAT SHOWS HOW MUCH AN ACTION/DAMAGE TAKES AWAY FROM CURRENT STAT)

        protected virtual void Awake()
        {
            Debug.LogError("UI_StatBar AWAKE");

            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();

            Debug.LogError("UI_StatBar slider: " + slider);
            Debug.LogError("UI_StatBar rectTransform:" + rectTransform);
        }

        public virtual void SetStat(int newValue)
        {
            Debug.LogError("SetStat CALLED, SetStat newValue = " + newValue);

            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            Debug.LogError("SetMaxStat CALLED, SetMaxStat maxValue = " + maxValue);

            slider.maxValue = maxValue;
            slider.value = maxValue;

            if (scaleBarLengthWithStats)
            {
                // SCALE THE TRANSFORM OF THIS OBJECT
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
            }
        }
    }
}

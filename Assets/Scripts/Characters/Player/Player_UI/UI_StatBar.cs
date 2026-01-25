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
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
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

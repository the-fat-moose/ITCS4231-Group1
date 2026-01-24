using UnityEngine;
using UnityEngine.UI;

namespace Group1 {
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;
        // VARIABLE TO SCALE BAR SIZE DEPENDING ON STAT (HIGHER STAT = HIGHER BAR ACROSS SCREEN)
        // SECONDARY BAR BEHIND MAIN BAR FOR POLISH EFFECT (YELLOW BAR THAT SHOWS HOW MUCH AN ACTION/DAMAGE TAKES AWAY FROM CURRENT STAT)

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
    }
}

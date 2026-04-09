using UnityEngine;

namespace Group1
{
    public class RotateLoadingIcon : MonoBehaviour
    {
        private float spinDuration = 2f;
        private float pauseDuration = 0.5f;
        private float spinSpeed = 360f;

        private float timer;

        void Update()
        {
            SpinIcon();
        }

        private void SpinIcon()
        {
            // INCREMENT TIMER BY DELTA TIME
            timer += Time.deltaTime;

            // CALCULATE TOTAL CYCLE TIME
            float totalCycle = spinDuration + pauseDuration;

            // CHECK IF WE ARE CURRENTLY IN THE SPIN PHASE
            if (timer < spinDuration)
            {
                // SPIN THE OBJECT
                float progress = timer / spinDuration;
                transform.localRotation = Quaternion.Euler(0, 0, -progress * spinSpeed);
            }
            else
            {
                // IN THE PAUSE PHASE, LOCK IT TO ITS ORIGINAL POSITION
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            // RESET THE TIMER ONCE THE FULL CYCLE IS DONE
            if (timer >= totalCycle)
            {
                timer = 0;
            }
        }
    }
}
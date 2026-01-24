using UnityEngine;

namespace Group1
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Player Health")]
        public int vigorLevel = 10;
        public int maxHealth;
        public int currentHealth;
        [SerializeField] private int vigorMultiplier = 10;

        [Header("UI Elements")]
        public HealthBar healthBar;

        /* ADD ANIMATOR REFERENCE HERE */

        private void Awake()
        {
            /* ADD ANIMATOR REFERENCE SET HERE */
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = vigorLevel * vigorMultiplier;

            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            if (healthBar != null) 
            {
                healthBar.SetCurrentHealth(currentHealth);
            }
            
            /* ADD GETTING HIT ANIMATION CODE HERE */

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                /* ADD DEATH ANIMATION CODE HERE */
                // HANDLE PLAYER DEATH (This will probably be related to the checkpoints)
            }
        }
    }
}
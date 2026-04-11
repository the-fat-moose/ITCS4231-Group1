using UnityEngine;

namespace Group1
{
    public class RangedEnemyCombatManager : AiCharacterCombatManager
    {
        [Header("Projectile")]
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] Transform projectileSpawnPoint;

        [Header("Damage")]
        [SerializeField] int baseDamage = 20;
        [SerializeField] float projectileDamageModifier = 1.0f;

        [Header("Attack Stats")]
        [SerializeField] float projectileSpeed = 20f;
        [SerializeField] float attackCooldown = 2f;

        public void FireProjectile()
        {
            if (projectilePrefab == null || projectileSpawnPoint == null)
                return;

            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            EnemyProjectile projectileComponent = projectile.GetComponent<EnemyProjectile>();
            if (projectileComponent != null)
            {
                projectileComponent.damage = Mathf.RoundToInt(baseDamage * projectileDamageModifier);
                projectileComponent.speed = projectileSpeed;
                projectileComponent.owner = this;
            }
        }

    }
}


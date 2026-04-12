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
            if (projectilePrefab == null || projectileSpawnPoint == null) return;

            //Force enemy to face the player before firing
            // Rotate enemy toward the player before firing
            Vector3 lookDir = currentTarget.characterCombatManager.lockOnTransform.position - transform.position;

            lookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDir);



            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            Transform aimPoint = currentTarget.characterCombatManager.lockOnTransform;

            Vector3 direction = (aimPoint.position - projectileSpawnPoint.position).normalized;
            projectile.transform.rotation = Quaternion.LookRotation(direction);



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


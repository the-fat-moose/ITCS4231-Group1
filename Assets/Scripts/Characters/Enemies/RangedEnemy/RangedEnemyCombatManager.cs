using Unity.VisualScripting;
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

        public void FireProjectile()
        {
            if (projectilePrefab == null || projectileSpawnPoint == null) return;

            //Force enemy to face the player before firing
            //Rotate enemy toward the player before firing
            Vector3 lookDir = currentTarget.characterCombatManager.lockOnTransform.position - transform.position;

            lookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDir);

            float playerSpeed = 0;
            Vector3 playerMoveDir = currentTarget.transform.forward;

            //Check if current target is player
            if (currentTarget.gameObject.TryGetComponent(out PlayerLocomotionManager playerLocomotion))
            {
                playerSpeed = playerLocomotion.moveAmount * 4f;
                playerMoveDir = playerLocomotion.worldMoveDir;
            }
            
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            //predict character movement
            Transform aimPoint = currentTarget.characterCombatManager.lockOnTransform;
            Vector3 playerPos = aimPoint.position;

            //Estimate player speed
            Vector3 playerVelocity = playerMoveDir * playerSpeed;

            //Distance to target
            float distance = Vector3.Distance(projectileSpawnPoint.position, playerPos);

            //Time for projectile to reach the target
            float travelTime = distance / projectileSpeed;

            //Predict future position
            Vector3 predictedPos = playerPos + playerVelocity * travelTime;

            Vector3 direction = (predictedPos  - projectile.transform.position).normalized;
            projectile.transform.rotation = Quaternion.LookRotation(direction);



            EnemyProjectile projectileComponent = projectile.GetComponent<EnemyProjectile>();
            if (projectileComponent != null)
            {
                projectileComponent.damage = Mathf.RoundToInt(baseDamage * projectileDamageModifier);
                projectileComponent.speed = projectileSpeed;
                projectileComponent.owner = GetComponent<CharacterManager>();
            }
        }

    }
}


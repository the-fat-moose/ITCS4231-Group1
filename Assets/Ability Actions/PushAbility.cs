using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Character Actions/Player Abilities/Push Ability")]
    public class PushAbility : PlayerAbilityAction
    {
        [Header("Push Settings")]
        public float range = 50f;
        public float width = 3.5f;
        public float height = 3f;
        public float pushForce = 10f;

        [Header("Animation")]
        public string pushAnimation = "PlayerCharacter_Ability_Push";

        public override void AttemptToPerformAbility(PlayerManager player)
        {
            base.AttemptToPerformAbility(player);

            if (player.isPerformingAction) return;
            if (!player.isGrounded) return;

            player.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.PushAbility, pushAnimation, true);
        }

        // Called by animation event
        public void ExecutePush(PlayerManager player)
        {
            Vector3 forward = player.transform.forward;
            Vector3 origin = player.transform.position + forward * (range * 0.5f);

            Quaternion rotation = Quaternion.LookRotation(forward);

            Collider[] hits = Physics.OverlapBox(origin, new Vector3(width * 0.5f, height * 0.5f, range * 0.5f), rotation, LayerMask.GetMask("Damageable Character"));

            foreach (var hit in hits)
            {
                CharacterManager character = hit.GetComponent<CharacterManager>();
                if (character == null || character.isDead) continue;

                Rigidbody rb = character.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 dir = (character.transform.position - player.transform.position).normalized;
                    character.ApplyKnockback(dir, pushForce);
                }
                
            }
        }

        public void DrawDebug(PlayerManager player)
        {
            Vector3 forward = player.transform.forward;
            Vector3 origin = player.transform.position + forward * (range * 0.5f);
            Quaternion rotation = Quaternion.LookRotation(forward);

            Matrix4x4 m = Matrix4x4.TRS(origin, rotation, Vector3.one);
            Gizmos.matrix = m;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, height, range));
        }
    }
}
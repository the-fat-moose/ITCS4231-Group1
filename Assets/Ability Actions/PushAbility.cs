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

        [Header("VFX")]
        public GameObject pushVFX;
        public Vector3 vfxOffset = new Vector3(0, 1f, 1f);

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
            Vector3 origin = player.transform.position + (forward * (range * 0.6f));

            Quaternion rotation = Quaternion.LookRotation(forward);

            if (pushVFX != null)
            {
                Vector3 spawnPos = player.transform.position + player.transform.forward * vfxOffset.z + player.transform.up * vfxOffset.y;

                Quaternion spawnRot = Quaternion.LookRotation(player.transform.forward);

                GameObject vfx = GameObject.Instantiate(pushVFX, spawnPos, spawnRot);
            }

            Collider[] hits = Physics.OverlapBox(origin, new Vector3(width * 0.5f, height * 0.5f, range * 0.5f), rotation, LayerMask.GetMask("Character"));

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
    }
}
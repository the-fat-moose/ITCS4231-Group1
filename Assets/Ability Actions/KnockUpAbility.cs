using Group1;
using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Character Actions/Player Abilities/Knock Up Ability")]
    public class KnockUpAbility : PlayerAbilityAction
    {
        [Header("VFX")]
        public GameObject knockUpVFX;
        public Vector3 vfxOffset = new Vector3(0, 1f, 1f);

        [Header("Animation")]
        public string knockUpAnimation = "PlayerCharacter_Ability_KnockUp";

        [Header("Knock Up Settings")]
        public float range = 4f;
        public float width = 3f;
        public float height = 3f;
        public float liftHeight = 4f;
        public float floatDuration = 1.2f;
        public float slamDamage = 30f;

        public override void AttemptToPerformAbility(PlayerManager player)
        {
            base.AttemptToPerformAbility(player);

            if (player.isPerformingAction) return;
            if (!player.isGrounded) return;

            player.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.KnockUpAbility, knockUpAnimation, true);
        }

        public void ExecuteKnockUp(PlayerManager player)
        {
            // Hitbox
            Vector3 forward = player.transform.forward;
            Vector3 origin = player.transform.position + forward * (range * 0.6f) + Vector3.up * (height * 0.5f);

            Quaternion rotation = Quaternion.LookRotation(forward);

            Collider[] hits = Physics.OverlapBox(origin, new Vector3(width * 0.5f, height * 0.5f, range * 0.5f), rotation, LayerMask.GetMask("Character"));
            Debug.LogError(hits.Length + " hit(s) detected in KnockUpAbility");
            foreach (var hit in hits)
            {
                Debug.Log("Applying KnockUpEffect");
                CharacterManager character = hit.GetComponent<CharacterManager>();
                if (character == null || character.isDead) continue;

                // Spawn VFX at enmemy
                if (knockUpVFX != null)
                {
                    Vector3 pos = character.transform.position + Vector3.up * 0.1f;

                    Quaternion rot = Quaternion.identity;
                    GameObject knockVfxObj = GameObject.Instantiate(knockUpVFX, pos, rot);

                    Destroy(knockVfxObj, 1.8f);
                }

                // Apply knock-up effect
                KnockUpEffect effect = Instantiate(WorldCharacterEffectsManager.instance.knockUpEffect);
                effect.liftHeight = liftHeight;
                effect.floatDuration = floatDuration;
                effect.slamDamage = slamDamage;
                Debug.LogError($"Applying KnockUpEffect to {character.name}");
                character.characterEffectsManager.ProcessInstantEffect(effect);
            }

        }

        public void DrawDebug(PlayerManager player)
        {
            Vector3 forward = player.transform.forward;
            Vector3 origin = player.transform.position + forward * (range * 0.6f);
            Quaternion rotation = Quaternion.LookRotation(forward);

            Matrix4x4 m = Matrix4x4.TRS(origin, rotation, Vector3.one);
            Gizmos.matrix = m;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, height, range));
        }
    }
}

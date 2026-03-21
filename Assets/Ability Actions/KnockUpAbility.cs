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
            // Spawn VFX at player
            if (knockUpVFX != null)
            {
                Vector3 pos = player.transform.position 
                            + player.transform.forward * vfxOffset.z
                            + Vector3.up * vfxOffset.y;

                Quaternion rot = Quaternion.LookRotation(player.transform.forward);
                GameObject.Instantiate(knockUpVFX, pos, rot);
            }

            // Hitbox
            Vector3 forward = player.transform.forward;
            Vector3 origin = player.transform.position + forward * (range * 0.5f);
            Quaternion rotation = Quaternion.LookRotation(forward);

            Collider[] hits = Physics.OverlapBox(
                origin,
                new Vector3(width * 0.5f, height * 0.5f, range * 0.5f),
                rotation,
                LayerMask.GetMask("Damageable Character")
            );

            foreach (var hit in hits)
            {
                CharacterManager character = hit.GetComponent<CharacterManager>();
                if (character == null || character.isDead) continue;

                // Apply knock-up effect
                KnockUpEffect effect = Instantiate(WorldCharacterEffectsManager.instance.knockUpEffect);
                effect.liftHeight = liftHeight;
                effect.floatDuration = floatDuration;
                effect.slamDamage = slamDamage;

                character.characterEffectsManager.ProcessInstantEffect(effect);
            }
        }
    }
}

using PixPlays.ElementalVFX;
using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Character Actions/Player Abilities/Cage Ability")]
    public class CageAbility : PlayerAbilityAction
    {
        [Header("VFX")]
        public GameObject cageVFX;
        public Vector3 vfxOffset = new Vector3(0, 1f, 1f);

        [Header("Animation")]
        public string cageAnimation = "PlayerCharacter_Ability_Freeze";

        
        [Header("Freeze Settings")]
        public float range = 5f;
        public float width = 3f;
        public float height = 3f;
        public float freezeDuration = 3f;

        public override void AttemptToPerformAbility(PlayerManager player)
        {
            base.AttemptToPerformAbility(player);

            if(player.isPerformingAction) return;
            if(!player.isGrounded) return;
            if(player.CurrentMana < player.playerCombatManager.cageAbilityManaCost) return;

            player.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.CageAbility, cageAnimation, true);
        }

        public void ExecuteCage(PlayerManager player)
        {
            // Hitbox
            Vector3 forward = player.transform.forward;
            Vector3 origin = player.transform.position + forward * (range * 0.6f);
            Quaternion rotation = Quaternion.LookRotation(forward);

            Collider[] hits = Physics.OverlapBox(origin, new Vector3(width * 0.5f, height * 0.5f, range * 0.5f), rotation, LayerMask.GetMask("Character"));

            foreach (var hit in hits)
            {
                Debug.LogError("Start of Foreach");
                CharacterManager character = hit.GetComponent<CharacterManager>();
                if (character == null || character.isDead) continue;

                // Spawn VFX at enemy feet
                if (cageVFX != null)
                {
                    Vector3 pos = character.transform.position;
                    Quaternion rot = Quaternion.identity; // or face player if you want

                    GameObject cageObj = GameObject.Instantiate(cageVFX, pos, rot);

                    Destroy(cageObj, freezeDuration + 0.3f); // Destroy VFX after freeze duration
                }

                // Apply freeze effect
                FreezeEffect effect = Instantiate(WorldCharacterEffectsManager.instance.freezeEffect);
                effect.duration = freezeDuration;

                character.characterEffectsManager.ProcessInstantEffect(effect);

                Debug.LogError("End of Foreach");
                character.characterEffectsManager.ProcessInstantEffect(effect);
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

using System.Collections.Generic;
using UnityEngine;

namespace Group1 {
    public class AIFinalBossCombatManager : AiCharacterCombatManager
    {
        AIFinalBossCharacterManager aiFinalBossCharacter;

        [Header("Damage Collider")]
        [SerializeField] FinalBossMeleeWeaponDamageCollider finalBossMeleeWeaponDamageCollider;

        [Header("Damage")]
        [SerializeField] int baseDamage = 25;
        [SerializeField] float attack01DamageModifier = 1.0f;
        [SerializeField] float attack02DamageModifier = 1.4f;

        
        [Header("Special Attacks")]
        [SerializeField] float circleAOEDistanceInFront = 2;
        [SerializeField] float circleAOERadius = 2;
        [SerializeField] int circleAOEMagicDamage = 25;

        [Header("Special Attack VFXs")]
        [SerializeField] GameObject circleAOEVFX;

        protected override void Awake()
        {
            base.Awake();

            aiFinalBossCharacter = GetComponent<AIFinalBossCharacterManager>();
        }

        public void SetAttack01Damage()
        {
            finalBossMeleeWeaponDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        }

        public void SetAttack02Damage()
        {
            finalBossMeleeWeaponDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        }

        public void ActivateFinalBossCircleAOE()
        {
            Vector3 circleAOECenterPosition = aiFinalBossCharacter.gameObject.transform.position + transform.forward * circleAOEDistanceInFront;

            // SHOW VFX
            GameObject circleAOEObject = Instantiate(circleAOEVFX, circleAOECenterPosition, Quaternion.identity);
            circleAOEObject.transform.localScale *= 2;

            // DEAL DAMAGE AND IGNORE BOSS
            Collider[] colliders = Physics.OverlapSphere(circleAOECenterPosition, circleAOERadius, WorldUtilityManager.Instance.GetCharacterLayers());

            List<CharacterManager> charactersDamaged = new List<CharacterManager>();

            foreach (var collider in colliders)
            {
                CharacterManager character = collider.GetComponentInParent<CharacterManager>();

                if (character == aiFinalBossCharacter) continue; // IGNORE BOSS

                if (character != null)
                {
                    if (charactersDamaged.Contains(character)) continue;

                    charactersDamaged.Add(character);

                    TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                    damageEffect.magicDamage = circleAOEMagicDamage;

                    character.characterEffectsManager.ProcessInstantEffect(damageEffect);
                }
            }

            // DESTROY VFX
            Destroy(circleAOEObject, 1f);
        }

        /*public void ActivateFinalBossLineAOE()
        {
            Collider
        }*/

        public void OpenFinalBossMeleeWeaponDamageCollider()
        {
            finalBossMeleeWeaponDamageCollider.EnableCollider();
        }

        public void CloseFinalBossMeleeWeaponDamageCollider()
        {
            finalBossMeleeWeaponDamageCollider.DisableCollider();
        }
    }
}
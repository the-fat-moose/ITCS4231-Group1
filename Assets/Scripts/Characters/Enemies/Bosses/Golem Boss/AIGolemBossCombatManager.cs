using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Group1
{
    public class AIGolemBossCombatManager : AiCharacterCombatManager
    {
        [Header("Character Reference")]
        [SerializeField] AIBossCharacterManager aiGolem;

        [Header("Melee Punch")]
        [SerializeField] FinalBossMeleeWeaponDamageCollider finalBossMeleeWeaponDamageCollider;

        [Header("Damage Values")]
        [SerializeField] int stompPhysicalDamage = 30;
        [SerializeField] int slamPhysicalDamage = 35;

        [Header("Stomp AOE")]
        [SerializeField] Transform stompOrigin;
        [SerializeField] float stompMaxRadius = 6f;
        [SerializeField] float stompExpandDuration = 0.45f;
        [SerializeField] GameObject stompExpandVFX;

        [Header("Slam Cone AOE")]
        [SerializeField] GameObject slamStartVFX;
        [SerializeField] float slamStartDistanceInFront = 2f;
        [SerializeField] float slamWidth = 3f;
        [SerializeField] float slamHeight = 2f;
        [SerializeField] float slamLength = 4f;

        protected override void Awake()
        {
            base.Awake();
            aiGolem = GetComponent<AIBossCharacterManager>();
        }

        public void ActivateGolemSlamAOE()
        {
            Vector3 center = aiGolem.transform.position + aiGolem.transform.forward * slamStartDistanceInFront;
            Quaternion rotation = Quaternion.LookRotation(aiGolem.transform.forward);

            // VFX
            if (slamStartVFX != null)
            {
                GameObject vfx = Instantiate(slamStartVFX, center, rotation);
                Destroy(vfx, 3f);
            }

            // Cone approximated by a wide box
            Vector3 halfExtents = new Vector3(slamWidth, slamHeight, slamLength);

            Collider[] colliders = Physics.OverlapBox(
                center,
                halfExtents,
                rotation,
                WorldUtilityManager.Instance.GetCharacterLayers()
            );

            ApplyDamageToColliders(colliders, slamPhysicalDamage);
        }

        public void ActivateGolemStompAOE()
        {
            if (stompOrigin == null)
            {
                Debug.LogWarning("Stomp Origin not assigned!");
                return;
            }

            StartCoroutine(StompExpandRoutine());
        }

        private IEnumerator StompExpandRoutine()
        {
            float timer = 0f;
            float currentRadius = 0f;

            // Spawn VFX
            if (stompExpandVFX != null)
            {
                GameObject vfx = Instantiate(stompExpandVFX, stompOrigin.position, Quaternion.identity);
                Destroy(vfx, 3f);
            }

            HashSet<CharacterManager> damaged = new HashSet<CharacterManager>();

            while (timer < stompExpandDuration)
            {
                timer += Time.deltaTime;
                currentRadius = Mathf.Lerp(0f, stompMaxRadius, timer / stompExpandDuration);

                Collider[] colliders = Physics.OverlapSphere(
                    stompOrigin.position,
                    currentRadius,
                    WorldUtilityManager.Instance.GetCharacterLayers()
                );

                foreach (var col in colliders)
                {
                    CharacterManager character = col.GetComponentInParent<CharacterManager>();
                    if (character == null || character == aiGolem) continue;
                    if (damaged.Contains(character)) continue;

                    damaged.Add(character);

                    TakeDamageEffect dmg = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                    dmg.physicalDamage = stompPhysicalDamage;
                    character.characterEffectsManager.ProcessInstantEffect(dmg);
                }

                yield return null;
            }
        }

        private void ApplyDamageToColliders(Collider[] colliders, int damage)
        {
            HashSet<CharacterManager> damaged = new HashSet<CharacterManager>();

            foreach (var col in colliders)
            {
                CharacterManager character = col.GetComponentInParent<CharacterManager>();
                if (character == null) continue;
                if (character == aiGolem) continue;
                if (damaged.Contains(character)) continue;

                damaged.Add(character);

                TakeDamageEffect dmg = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                dmg.physicalDamage = damage;
                character.characterEffectsManager.ProcessInstantEffect(dmg);
            }
        }

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


using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Group1
{
    public class AIGolemBossCombatManager : AiCharacterCombatManager
    {
        [SerializeField] AIFinalBossCharacterManager aiGolem;

        [Header("Damage")]
        [SerializeField] int stompPhysicalDamage = 30;
        [SerializeField] int slamPhysicalDamage = 35;

        [Header("Stomp AOE")]
        [Required]
        [SerializeField] Transform stompOrigin;      

        [SerializeField] float stompRadius = 3f;
        [SerializeField] GameObject stompVFX;

        [Header("Slam Cone AOE")]
        [SerializeField] GameObject slamStartVFX;
        [SerializeField] GameObject slamRollingVFX;

        [SerializeField] float slamStartDistanceInFront = 2f;
        [SerializeField] float slamStepForwardDistance = 2.5f;
        [SerializeField] int slamSteps = 3;

        //Medium cone width: grows 2 -> 4 -> 6
        [SerializeField] float slamStartWidth = 2f;
        [SerializeField] float slamEndWidth = 6f;

        [SerializeField] float slamHeight = 2f;
        [SerializeField] float slamStepDelay = 0.35f;
        [SerializeField] float slamInitialDelay = 0.25f;

        protected override void Awake()
        {
            base.Awake();
            aiGolem = GetComponent<AIFinalBossCharacterManager>();
        }

        //STOMP ATTACK
        public void ActivateGolemStompAOE()
        {
            if (stompOrigin == null)
            {
                Debug.LogWarning("Stomp Origin not assigned!");
                return;
            }

            Vector3 center = stompOrigin.position;

            // VFX
            if (stompVFX != null)
            {
                GameObject vfx = Instantiate(stompVFX, center, Quaternion.identity);
                Destroy(vfx, 2f);
            }

            Collider[] colliders = Physics.OverlapSphere(center, stompRadius, WorldUtilityManager.Instance.GetCharacterLayers());

            List<CharacterManager> damagedCharacters = new List<CharacterManager>();

            foreach (var col in colliders)
            {
                CharacterManager character = col.GetComponentInParent<CharacterManager>();
                if (character == null) continue;
                if (character == aiGolem) continue;
                if (damagedCharacters.Contains(character)) continue;

                damagedCharacters.Add(character);

                TakeDamageEffect dmg = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                dmg.physicalDamage = stompPhysicalDamage;

                character.characterEffectsManager.ProcessInstantEffect(dmg);
            }
        }

        //SLAM CONE ATTACK
        public void ActivateGolemSlamAOE()
        {
            Vector3 startPoint = aiGolem.transform.position + aiGolem.transform.forward * slamStartDistanceInFront;
            Quaternion rotation = Quaternion.LookRotation(aiGolem.transform.forward);

            // Start VFX
            if (slamStartVFX != null)
            {
                GameObject startVFX = Instantiate(slamStartVFX, startPoint, rotation);
                Destroy(startVFX, 2f);
            }

            StartCoroutine(SlamConeRoutine(startPoint, rotation));
        }

        private IEnumerator SlamConeRoutine(Vector3 startPos, Quaternion rotation)
        {
            yield return new WaitForSeconds(slamInitialDelay);

            float widthStep = (slamEndWidth - slamStartWidth) / slamSteps;
            float currentWidth = slamStartWidth;

            for (int i = 0; i < slamSteps; i++)
            {
                Vector3 halfExtents = new Vector3(currentWidth, slamHeight, 1f);

                Collider[] colliders = Physics.OverlapBox(
                    startPos,
                    halfExtents,
                    rotation,
                    WorldUtilityManager.Instance.GetCharacterLayers()
                );

                // Rolling VFX
                if (slamRollingVFX != null)
                {
                    GameObject rollVFX = Instantiate(
                        slamRollingVFX,
                        startPos + new Vector3(0, 0.1f, 0),
                        rotation
                    );
                    Destroy(rollVFX, 5f);
                }

                List<CharacterManager> damagedCharacters = new List<CharacterManager>();

                foreach (var col in colliders)
                {
                    CharacterManager character = col.GetComponentInParent<CharacterManager>();
                    if (character == null) continue;
                    if (character == aiGolem) continue;
                    if (damagedCharacters.Contains(character)) continue;

                    damagedCharacters.Add(character);

                    TakeDamageEffect dmg = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                    dmg.physicalDamage = slamPhysicalDamage;

                    character.characterEffectsManager.ProcessInstantEffect(dmg);
                }

                // Move forward for next step
                startPos += aiGolem.transform.forward * slamStepForwardDistance;
                currentWidth += widthStep;

                yield return new WaitForSeconds(slamStepDelay);
            }
        }
    }
}


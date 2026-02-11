using UnityEngine;

namespace Group1 {
    public class AiCharacterCombatManager : CharacterCombatManager
    {
        [Header("Detection")]
        [SerializeField] float detectionRadius = 15f;

        private void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
        {
            if (currentTarget != null) return;

            Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.Instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                if (targetCharacter == null) continue;

                if (targetCharacter == aiCharacter) continue;

                if (targetCharacter.isDead) continue;

                //if (WorldUtilityManager.Instance.CanIDamageThisTarget(aiCharacter, targetCharacter))
            }
        }
    }
}
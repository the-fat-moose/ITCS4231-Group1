using System.Collections;
using UnityEngine;

namespace Group1
{
    public class AIFinalBossCharacterManager : AIBossCharacterManager
    {
        public AIFinalBossSoundFXManager finalBossSoundFXManager;

        protected override void Awake()
        {
            base.Awake();

            finalBossSoundFXManager = GetComponent<AIFinalBossSoundFXManager>();
        }

        public override IEnumerator ProcessDeathEvent()
        {
            ArtifactInteractable artifactInteractable = FindFirstObjectByType<ArtifactInteractable>();
            if (artifactInteractable != null) artifactInteractable.EnableTriggerCollider();

            return base.ProcessDeathEvent();
        }
    }
}
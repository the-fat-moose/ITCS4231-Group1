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
    }
}
using Group1;
using UnityEngine;

namespace Group1
{
   public class AIGolemBossCharacterManager : AIBossCharacterManager
    {
        public AIFinalBossSoundFXManager finalBossSoundFXManager;

        protected override void Awake()
        {
            base.Awake();

            finalBossSoundFXManager = GetComponent<AIFinalBossSoundFXManager>();
        }
    } 
}


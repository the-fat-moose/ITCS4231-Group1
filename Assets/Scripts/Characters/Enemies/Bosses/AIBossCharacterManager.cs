using System.Collections;
using UnityEngine;

namespace Group1 {
    public class AIBossCharacterManager : AICharacterManager
    {
        // GIVE THE AI A UNIQUE ID
        public int bossID = 0;
        [SerializeField] bool hasBeenDefeated = false;

        // WHEN THE AI IS SPAWNED, CHECK IF THE BOSS HAS BEEN DEFEATED
        // IF THE BOSS HAS BEEN DEFEATED, DISABLE THIS OBJECT
        // IF THE HAS NOT BEEN DEFEATED, DO NOT DISABLE IT
        // HANDLE TRIGGERS WHEN THE BOSS HAS BEEN INTERACTED WITH AT LEAST ONCE

        [Header("TEST")]
        [SerializeField] bool defeatedBossDebug = false;

        protected override void Start()
        {
            base.Start();

            // IF OUR SAVE DATA DOES NOT CONTAIN INFO ON THIS BOSS, ADD IT
            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, false);
            }
            // OTHERWISE, LOAD THE DATA THAT ALREADY EXISTS ON THIS BOSS
            else
            {
                hasBeenDefeated = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];

                if (hasBeenDefeated)
                {
                    IsActive = false;
                }
            }
        }

        public override IEnumerator ProcessDeathEvent()
        {
            CurrentHealth = 0;
            isDead = true;

            // RESET ANY FLAGS HERE THAT NEED TO BE RESET
            // NOTHING YET

            characterAnimatorManager.PlayTargetActionAnimation("Death", true);

            hasBeenDefeated = true;
            // IF OUR SAVE DATA DOES NOT CONTAIN INFO ON THIS BOSS, ADD IT
            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
            }
            // OTHERWISE, LOAD THE DATA THAT ALREADY EXISTS ON THIS BOSS
            else
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
            }

            WorldSaveGameManager.instance.SaveGame();

            // PLAY SOME DEATH SFX

            yield return new WaitForSeconds(5f);
            
            // DISABLE CHARACTER
        }
    }
}
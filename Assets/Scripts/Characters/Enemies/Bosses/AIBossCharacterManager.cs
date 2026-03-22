using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Group1 {
    public class AIBossCharacterManager : AICharacterManager
    {
        // GIVE THE AI A UNIQUE ID
        public int bossID = 0;

        [Header("Status")]
        [SerializeField] private bool hasBeenDefeated = false;
        [SerializeField] private bool hasBeenAwakened = false;
        [SerializeField] string sleepAnimation;
        [SerializeField] string awakenAnimation;

        [Header("States")]
        [SerializeField] BossSleepState sleepState;

        [Header("Fog Wall")]
        [SerializeField] private List<FogWallInteractable> fogWalls;

        // WHEN THE AI IS SPAWNED, CHECK IF THE BOSS HAS BEEN DEFEATED
        // IF THE BOSS HAS BEEN DEFEATED, DISABLE THIS OBJECT
        // IF THE HAS NOT BEEN DEFEATED, DO NOT DISABLE IT
        // HANDLE TRIGGERS WHEN THE BOSS HAS BEEN INTERACTED WITH AT LEAST ONCE

        protected override void Awake()
        {
            base.Awake();

            sleepState = Instantiate(sleepState);
            currentState = sleepState;
        }

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
                hasBeenAwakened = WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID];
            }

            // LOCATE FOG WALL
            StartCoroutine(GetFogWallsFromWorldObjectManager());

            // IF THE BOSS HAS BEEN AWAKENED, ENABLE THE FOG WALLS
            if (hasBeenAwakened)
            {
                for (int i = 0; i < fogWalls.Count; i++)
                {
                    fogWalls[i].IsActive = true;
                }
            }

            // IF THE BOSS HAS BEEN DEFEATED, DISABLE THE FOG WALLS
            if (hasBeenDefeated)
            {
                for (int i = 0; i < fogWalls.Count; i++)
                {
                    fogWalls[i].IsActive = false;
                }

                IsActive = false;
            }

            if (!hasBeenAwakened)
            {
                characterAnimatorManager.PlayTargetActionAnimation(sleepAnimation, true);
            }
        }

        private IEnumerator GetFogWallsFromWorldObjectManager()
        {
            while (WorldObjectManager.instance.fogWalls.Count == 0)
            {
                yield return new WaitForEndOfFrame();
            }

            fogWalls = new List<FogWallInteractable>();

            // LOCATE FOG WALL
            foreach (var fogWall in WorldObjectManager.instance.fogWalls)
            {
                if (fogWall.fogWallID == bossID)
                {
                    fogWalls.Add(fogWall);
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
    
        public void WakeBoss()
        {
            if (!hasBeenAwakened)
            {
                characterAnimatorManager.PlayTargetActionAnimation(awakenAnimation, true);
            }

            hasBeenAwakened = true;
            currentState = idle;

            // IF OUR SAVE DATA DOES NOT CONTAIN INFO ON THIS BOSS, ADD IT
            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            }
            // OTHERWISE, LOAD THE DATA THAT ALREADY EXISTS ON THIS BOSS
            else
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            }

            for (int i = 0; i < fogWalls.Count; i++)
            {
                fogWalls[i].IsActive = true;
            }
        }
    }
}
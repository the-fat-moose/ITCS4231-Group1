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
    }
}
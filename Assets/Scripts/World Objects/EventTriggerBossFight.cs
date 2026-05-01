using UnityEngine;

namespace Group1
{
    public class EventTriggerBossFight : MonoBehaviour
    {
        [SerializeField] int bossID;

        void OnTriggerEnter(Collider other)
        {
            AIBossCharacterManager boss = WorldAIManager.instance.GetBossCharacterByID(bossID);
            
            Debug.Log("BOSS: " + boss.bossID);
            Debug.Log("BOSS: " + boss.hasBeenDefeated);

            if (boss != null && !boss.hasBeenDefeated)
            {
                // Testing
            }

            boss.WakeBoss();
            this.gameObject.SetActive(false);
        }
    }
}
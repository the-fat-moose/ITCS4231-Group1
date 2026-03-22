using UnityEngine;

namespace Group1
{
    public class EventTriggerBossFight : MonoBehaviour
    {
        [SerializeField] int bossID;

        void OnTriggerEnter(Collider other)
        {
            AIBossCharacterManager boss = WorldAIManager.instance.GetBossCharacterByID(bossID);

            if (boss != null)
            {
                boss.WakeBoss();
            }
        }
    }
}
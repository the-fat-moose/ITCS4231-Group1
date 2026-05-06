using System.Collections;
using System.Linq;
using Group1;
using Unity.VisualScripting;
using UnityEngine;

namespace Group1
{
    public class AIGolemBossCharacterManager : AIBossCharacterManager
    {
        public AIFinalBossSoundFXManager finalBossSoundFXManager;

        private GameObject caveExit;
        private GameObject lightAtEndOfTunnel;
        private GameObject heartstone;
        private GameObject voidRend;

        protected override void Awake()
        {
            base.Awake();
            finalBossSoundFXManager = GetComponent<AIFinalBossSoundFXManager>();
        }

        protected override void Start()
        {
            base.Start();

            // The spawner is now active, so this works
            var spawner = GetComponentInParent<AICharacterSpawner>();
            if (spawner == null)
            {
                Debug.LogError("Golem boss could not find its spawner.");
                return;
            }

            // Find the environment linker under the spawner
            var provider = spawner.GetComponentInChildren<BossEnvironmentLinker>(true);
            if (provider == null)
            {
                Debug.LogError("BossEnvironmentLinker not found under spawner.");
                return;
            }

            caveExit = provider.caveExit;
            lightAtEndOfTunnel = provider.lightAtEndOfTunnel;
            heartstone = provider.heartstone;
            voidRend = provider.voidRend;

            Debug.Log("Golem boss successfully linked to environment objects.");
        }

        public override IEnumerator ProcessDeathEvent()
        {
            yield return StartCoroutine(base.ProcessDeathEvent());
            OpenPath();
        }

        private void OpenPath()
        {
            caveExit.SetActive(false);
            lightAtEndOfTunnel.SetActive(true);
            heartstone.SetActive(true);
            voidRend.SetActive(true);
        }
    }
}



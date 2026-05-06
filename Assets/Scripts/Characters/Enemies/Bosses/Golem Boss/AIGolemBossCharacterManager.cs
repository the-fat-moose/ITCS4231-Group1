using System.Collections;
using Group1;
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

            caveExit = GameObject.FindWithTag("CaveExit");
            lightAtEndOfTunnel = GameObject.FindWithTag("TunnelLight");
            heartstone = GameObject.FindWithTag("Heartstone");
            voidRend = GameObject.FindWithTag("Voidrend");
            
        }
        virtual public void Start()
        {
            base.Start();
            if(lightAtEndOfTunnel != null) heartstone.SetActive(false);
            
            if(heartstone != null) heartstone.SetActive(false);
            
            if(voidRend != null) voidRend.SetActive(false);
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


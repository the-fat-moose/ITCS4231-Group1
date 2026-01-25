using UnityEngine;

namespace Group1 {
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            CalculateHealthBasedOnVitalityLevel(player.Vitality);
            CalculateStaminaBasedOnEnduranceLevel(player.Endurance);
        }
    }
}
using UnityEngine;
using TMPro;

namespace Group1
{
    public class UI_Boss_HP_Bar : UI_StatBar
    {
        [SerializeField] AIBossCharacterManager bossCharacter;

        public void EnableBossHPBar(AIBossCharacterManager boss)
        {
            bossCharacter = boss;
            bossCharacter.OnHealthChanged += OnBossHPChanged;
            SetMaxStat(bossCharacter.MaxHealth);
            SetStat(bossCharacter.CurrentHealth);

            GetComponentInChildren<TextMeshProUGUI>().text = bossCharacter.characterName;
        }

        private void OnDestroy()
        {
            bossCharacter.OnHealthChanged -= OnBossHPChanged;
        }

        private void OnBossHPChanged(int oldValue, int newValue)
        {
            SetStat(newValue);

            if (newValue <= 0)
            {
                RemoveHPBar(2.5f);
            }
        }

        public void RemoveHPBar(float time)
        {
            Destroy(gameObject, time);
        }
    }
}
using UnityEngine;

namespace Group1 {
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("Debug Delete Later")]
        [SerializeField] InstantCharacterEffect effectToTest;
        [SerializeField] bool processEffect = false;

        private void Update()
        {
            if (processEffect)
            {
                InstantCharacterEffect effect = Instantiate(effectToTest);
                ProcessInstantEffect(effect);
                processEffect = false;
            }
        }
    }
}
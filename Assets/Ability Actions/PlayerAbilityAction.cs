using UnityEngine;

namespace Group1
{
    public abstract class PlayerAbilityAction : ScriptableObject
    {
        public int actionID;

        public virtual void AttemptToPerformAbility(PlayerManager player)
        {
            Debug.LogError("Ability Fired: " + name);
        }
    }
}

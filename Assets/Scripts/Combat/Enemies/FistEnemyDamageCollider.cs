using UnityEngine;

namespace Group1 {
    public class FistEnemyDamageCollider : DamageCollider
    {
        AICharacterManager aiCharacterCausingDamage;

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget)) return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }
    }
}
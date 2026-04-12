using Group1;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage;
    public float speed;
    public AiCharacterCombatManager owner;

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterManager target = other.GetComponent<CharacterManager>();
        if (target != null && target != owner)
        {
            TakeDamageEffect dmg = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            dmg.magicDamage = damage;
            target.characterEffectsManager.ProcessInstantEffect(dmg);
        }

        Destroy(gameObject);
    }
}

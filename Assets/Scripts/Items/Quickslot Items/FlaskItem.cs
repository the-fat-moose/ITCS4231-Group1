using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Items/Consumables/Life Shard")]
    public class FlaskItem : QuickSlotItem
    {
        [Header("Empty Item")]
        [SerializeField] GameObject emptyFlaskModel;
    }
}
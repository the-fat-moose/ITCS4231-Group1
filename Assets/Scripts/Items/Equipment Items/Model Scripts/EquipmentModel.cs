using UnityEngine;

namespace Group1
{
    [CreateAssetMenu(menuName = "Equipment Model")]
    public class EquipmentModel : ScriptableObject
    {
        // TYPE
        public EquipmentModelType equipmentModelType;

        // NAME
        public string equipmentName; 

        public void LoadModel(PlayerManager player)
        {
            // SEARCH THROUGH A LIST OF ALL EQUIPMENT MODELS BASED ON TYPE
            // ENABLE THE MODEL THAT MATCHES THE NAME

            switch (equipmentModelType)
            {
                case EquipmentModelType.Lumen:
                    foreach (var model in player.playerEquipmentManager.lumenSlot01Array)
                    {
                        if (model.gameObject.name == equipmentName)
                        {
                            model.gameObject.SetActive(true);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
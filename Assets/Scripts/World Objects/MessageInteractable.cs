using UnityEngine;

namespace Group1
{
    public class MessageInteractable : Interactable
    {
        [Header("Message")]
        [SerializeField] private string messagePopUp;

        public override void Interact(PlayerManager player)
        {
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

            PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp(messagePopUp);
        }
    }
}
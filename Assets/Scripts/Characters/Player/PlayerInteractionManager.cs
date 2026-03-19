using System.Collections.Generic;
using UnityEngine;

namespace Group1 {
    public class PlayerInteractionManager : MonoBehaviour
    {
        PlayerManager player;

        private List<Interactable> currentInteractableActions;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            currentInteractableActions = new List<Interactable>();
        }

        private void FixedUpdate()
        {
            // IF OUR UI MENU IS NOT OPEN, AND WE DONT HAVE A POP UP CURRENTLY ON THE SCREEN, CHECK FOR INTERACTABLE
            if (!PlayerUIManager.instance.menuWindowIsOpen && !PlayerUIManager.instance.popUpWindowIsOpen)
            {
                CheckForInteractable();
            }
        }

        private void CheckForInteractable()
        {
            if (currentInteractableActions.Count == 0) return;

            if (currentInteractableActions[0] == null)
            {
                currentInteractableActions.RemoveAt(0);
                return;
            }

            // IF WE HAVE AN INTERACTABLE ACTION AND HAVE NOT NOTIFIED OUR PLAYER, WE DO SO HERE
            if (currentInteractableActions[0] != null)
            {
                PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp(currentInteractableActions[0].interactableText);
            }
        }
    }
}
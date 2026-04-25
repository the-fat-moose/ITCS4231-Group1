using UnityEngine;

namespace Group1 {
    public class Interactable : MonoBehaviour
    {
        // WHAT ARE INTERACTABLES? (ANYTHING YOU CAN INTERACT WITH IN THE WORLD)
        // LEVERS
        // ITEMS
        // FOG WALLS
        // ELEVATORS

        public string interactableText; // TEXT PROMPT WHEN ENTERING THE INTERACTION COLLIDER OnTriggerEnter
        public Collider interactableCollider;
        
        protected virtual void Awake()
        {
            // CHECK IF THE COLLIDER IS NULL
            if (interactableCollider == null) interactableCollider = GetComponent<Collider>();
        }

        protected virtual void Start()
        {
            
        }

        public virtual void Interact(PlayerManager player)
        {
            interactableCollider.enabled = false;
            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

            WorldSaveGameManager.instance.SaveGame();
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                // PASS THE INTERACTION TO THE PLAYER
                player.playerInteractionManager.AddInteractionToList(this);
            }
        }

        public virtual void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                // REMOVE THE INTERACTION FROM THE PLAYER
                player.playerInteractionManager.RemoveInteractionFromList(this);

                PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            }
        }
    }
}
using UnityEngine;

namespace Group1 {
    public class DoorInteractable : Interactable
    {
        [Header("Status")]
        private bool isOpen = false;
        public event System.Action<bool, bool> OnIsOpenValueChanged;

        public bool IsOpen
        {
            get => isOpen;
            set
            {
                if (isOpen == value) return;

                bool oldValue = isOpen;
                isOpen = value;
                OnIsOpenValueChanged?.Invoke(oldValue, isOpen);
            }
        }
    
        [SerializeField] private string doorID;

        // CANNOT OPEN FROM THIS SIDE INTERACTABLE

        // ITEM REQUIRED (KEY)
        [Header("Requires Item")]
        [SerializeField] private bool requiresItem = false;
        [SerializeField] private Item itemRequiredToOpen;

        // DOOR ANIMATION
        [Header("Animation")]
        [SerializeField] private Animator animator;
        [SerializeField] private string openDoorAnimation;
        [SerializeField] private string openedDoorAnimation;

        // SFX
        [Header("SFX")]
        [SerializeField] AudioSource audioSource;
        [SerializeField] private AudioClip doorOpeningSFX;

        protected override void Start()
        {
            base.Start();

            // PROVIDING YOUR DOORS ARE NOT NAMED THE SAME, THIS WILL GENERATE A UNIQUE ID FOR THEM
            doorID = gameObject.scene.buildIndex + " " + gameObject.name;

            for (int i = 0; i < WorldSaveGameManager.instance.currentCharacterData.doorsOpened.Count; i++)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.doorsOpened[i] == null) continue;

                if (WorldSaveGameManager.instance.currentCharacterData.doorsOpened[i] == doorID)
                { 
                    IsOpen = true;
                    break;
                }
            }

            // SUBCRIBE TO A FUNCTION ON SPAWN SO THAT IF THE DOOR IS OPENED, WE PLAN AN OPEN ANIMATION + FX
            OnIsOpenValueChanged += OnIsOpenChanged;

            // CHECK ON SPAWN INITIALLY, IF THE DOOR IS ALREADY OPENED, IF SO PLAYED THE "OPENED" ANIMATION INSTEAD OF THE "OPEN" ANIMATION
            CheckIfDoorIsAlreadyOpened();
        }

        public override void Interact(PlayerManager player)
        {
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

            // SAVE GAME AFTER INTERACTING
            WorldSaveGameManager.instance.SaveGame();

            if (requiresItem && PlayerHasItem(player))
            {
                OpenDoor();
                player.playerInteractionManager.RemoveInteractionFromList(this);
                PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp("Used " + itemRequiredToOpen.itemName + ".");
                player.playerInventoryManager.RemoveItemFromInventory(itemRequiredToOpen);
                return;
            }
            else if (requiresItem && !PlayerHasItem(player))
            {
                PlayerUIManager.instance.playerUIPopUpManager.SendPlayerMessagePopUp("Door locked.");
                return;
            }

            OpenDoor();
            player.playerInteractionManager.RemoveInteractionFromList(this);
        }

        private void OnIsOpenChanged(bool oldStatus, bool newStatus)
        {
            if (IsOpen)
            {
                interactableCollider.enabled = false;

                // DISABLED CANNOT OPEN FROM THIS SIDE COLLIDER
            }
        }

        private void CheckIfDoorIsAlreadyOpened()
        {
            if (IsOpen)
            {
                animator.Play(openedDoorAnimation);
                interactableCollider.enabled = false;

                // DISABLED CANNOT OPEN FROM THIS SIDE COLLIDER
            }
        }

        private void OnDisable()
        {
            OnIsOpenValueChanged -= OnIsOpenChanged;
        }

        private bool PlayerHasItem(PlayerManager player)
        {
            bool hasItem = false;

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                if (player.playerInventoryManager.itemsInInventory[i] == null) continue;

                if (player.playerInventoryManager.itemsInInventory[i].itemID == itemRequiredToOpen.itemID)
                {
                    hasItem = true;
                    break;
                }
            }

            return hasItem;
        }
    
        private void OpenDoor()
        {
            IsOpen = true;
            if (!WorldSaveGameManager.instance.currentCharacterData.doorsOpened.Contains(doorID))
                WorldSaveGameManager.instance.currentCharacterData.doorsOpened.Add(doorID);

            animator.Play(openDoorAnimation);
            audioSource.PlayOneShot(doorOpeningSFX);
            interactableCollider.enabled = false;
        }
    }
}
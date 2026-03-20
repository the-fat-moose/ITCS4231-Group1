using UnityEngine;

namespace Group1 {
    public class PickUpItemInteractable : Interactable
    {
        public ItemPickUpType pickUpType;

        [Header("Item")]
        [SerializeField] Item item;

        [Header("World Spawn Pick Up")]
        [SerializeField] int itemID;
        [SerializeField] bool hasBeenLooted = false;

        protected override void Start()
        {
            base.Start();

            if (pickUpType == ItemPickUpType.WorldSpawn)
                CheckIfWorldItemWasAlreadyLooted();
        }

        private void CheckIfWorldItemWasAlreadyLooted()
        {
            // COMPARE THE DATA OF LOOTED ITEMS I.D'S WITH THIS ITEM'S I.D
            if (!WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(itemID))
            {
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(itemID, false);
            }

            hasBeenLooted = WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted[itemID];

            // IF IT HAS BEEN LOOTED, HIDE THE GAME OBJECT
            if (hasBeenLooted)
                gameObject.SetActive(false);
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            // PLAY A SFX
            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.pickupItemSFX);

            // ADD ITEM TO INVENTORY
            player.playerInventoryManager.AddItemToInventory(item);

            // DISPLAY A UI POP UP SHOWING ITEM'S NAME AND PICTURE
            PlayerUIManager.instance.playerUIPopUpManager.SendItemPopUp(item, 1);

            // SAVE LOOT STATUS IF IT'S A WORLD SPAWN
            if (pickUpType == ItemPickUpType.WorldSpawn)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey((int)itemID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Remove(itemID);
                }

                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(itemID, true);
            }

            // HIDE OR DESTROY GAMEOBJECT
            Destroy(gameObject);
        }
    }
}
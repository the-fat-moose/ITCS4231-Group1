using System.Collections;
using UnityEngine;

namespace Group1
{
    public class GeodeInteractable : Interactable
    {
        [Header("Geode Info")]
        [SerializeField] int geodeID;

        [Header("Activated")]
        private bool isActivated = false;

        public event System.Action<bool, bool> OnIsActivatedValueChanged;

        public bool IsActivated
        {
            get => isActivated;
            set
            {
                if (isActivated == value) return;

                bool oldValue = isActivated;
                isActivated = value;
                OnIsActivatedValueChanged?.Invoke(oldValue, isActivated);
            }
        }

        [Header("VFX")]
        [SerializeField] GameObject activatedParticles;

        [Header("Interaction Text")]
        [SerializeField] private string unactivatedInteractionText = "Discover Geode";
        [SerializeField] private string activatedInteractionText = "Rest";

        protected override void Start()
        {
            base.Start();

            if (WorldSaveGameManager.instance.currentCharacterData.geodes.ContainsKey(geodeID))
            {
                IsActivated = WorldSaveGameManager.instance.currentCharacterData.geodes[geodeID];
            }
            else
            {
                IsActivated = false;
            }

            OnIsActivatedChanged(false, IsActivated);
            OnIsActivatedValueChanged += OnIsActivatedChanged;

            if (IsActivated)
            {
                interactableText = activatedInteractionText;
            }
            else
            {
                interactableText = unactivatedInteractionText;
            }
        }

        private void ShatterGeode(PlayerManager player)
        {
            // ADD GEODE TO ACTIVATED GEODES IN SAVE FILES
            IsActivated = true;

            if (WorldSaveGameManager.instance.currentCharacterData.geodes.ContainsKey(geodeID))
            {
                WorldSaveGameManager.instance.currentCharacterData.geodes.Remove(geodeID);
            }
            WorldSaveGameManager.instance.currentCharacterData.geodes.Add(geodeID, true);

            // PLAY AN ANIMATION
            player.playerAnimatorManager.PlayTargetActionAnimation("Activate_Geode_01", true);

            // SEND A POP UP
            PlayerUIManager.instance.playerUIPopUpManager.SendGeodeShatteredPopUp("GEODE DISCOVERED");

            // ENABLE/ACTIVATE THE GEODE
            StartCoroutine(WaitForAnimationAndPopUpThenRestoreCollider());
        }

        private void RestAtGeode(PlayerManager player)
        {
            Debug.Log("RESTING");
            interactableCollider.enabled = true;
            // RESTORE HEALTH, STAMINA, AND MANA
            player.CurrentHealth = player.MaxHealth;
            player.CurrentStamina = player.MaxStamina;
            player.CurrentMana = player.MaxMana;

            // REFILL FLASKS
            // RESET MONSTERS/CHARACTER LOCATIONS
            WorldAIManager.instance.ResetAllCharacters();
        }

        private IEnumerator WaitForAnimationAndPopUpThenRestoreCollider()
        {
            yield return new WaitForSeconds(3f);

            interactableCollider.enabled = true;
        }

        private void OnIsActivatedChanged(bool oldStatus, bool newStatus)
        {
            if (IsActivated)
            {
                // PLAY SOME FX HERE TO ENABLE A LIGHT OR SOMETHING TO INDICATE THE CHECKPOINT IS ON
                activatedParticles.SetActive(true);

                if (IsActivated)
                {
                    interactableText = activatedInteractionText;
                }
                else
                {
                    interactableText = unactivatedInteractionText;
                }
            }
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            if (!IsActivated)
            {
                ShatterGeode(player);
            }
            else
            {
                RestAtGeode(player);
            }
        }
    }
}
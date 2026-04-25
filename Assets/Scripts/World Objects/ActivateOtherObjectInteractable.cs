using System.Collections;
using UnityEngine;

namespace Group1
{
    public class ActivateOtherObjectInteractable : Interactable
    {
        [Header("Status")]
        private bool leverHasBeenPulled = false;
        public event System.Action<bool, bool> OnLeverHasBeenPulledValueChanged;

        public bool LeverHasBeenPulled
        {
            get => leverHasBeenPulled;
            set
            {
                if (leverHasBeenPulled == value) return;

                bool oldValue = leverHasBeenPulled;
                leverHasBeenPulled = value;
                OnLeverHasBeenPulledValueChanged?.Invoke(oldValue, leverHasBeenPulled);
            }
        }

        [Header("Interactable")]
        [SerializeField] Interactable interactableObject;

        [Header("Use Once")]
        [SerializeField] bool useOnce = true;

        [Header("Animator")]
        [SerializeField] Animator animator;
        [SerializeField] string pullLeverAnimation;
        [SerializeField] string resetLeverAnimation;
        [SerializeField] string pulledLeverAnimation;

        protected override void Start()
        {
            base.Start();

            if (LeverHasBeenPulled) animator.Play(pulledLeverAnimation);
        }

        public override void Interact(PlayerManager player)
        {
            PullLever(player);

            if (!useOnce)
            {
                StartCoroutine(WaitThenReactivateLever(3f));
            }

            WorldSaveGameManager.instance.SaveGame();

            if (interactableObject == null) return;

            interactableObject.Interact(player);
        }

        private void PullLever(PlayerManager player)
        {
            interactableCollider.enabled = false;
            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            animator.Play(pullLeverAnimation);

            LeverHasBeenPulled = true;
        }

        private IEnumerator WaitThenReactivateLever(float delay)
        {
            yield return new WaitForSeconds(delay);

            interactableCollider.enabled = true;
            animator.Play(resetLeverAnimation);
        }
    }
}
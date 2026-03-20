using System.Collections;
using UnityEngine;

namespace Group1 {
    public class FogWallInteractable : Interactable
    {
        [Header("Fog")]
        [SerializeField] private GameObject[] fogWallGameObjects;

        [Header("Collision")]
        [SerializeField] private Collider fogWallCollider;

        [Header("I.D")]
        public int fogWallID;

        [Header("Sound")]
        private AudioSource fogWallAudioSource;
        [SerializeField] private AudioClip fogWallSFX;

        [Header("Active")]
        private bool isActive = false;

        public event System.Action<bool, bool> OnIsActiveValueChanged;

        public bool IsActive
        {
            get => isActive;
            set
            {
                if (isActive == value) return;

                bool oldValue = isActive;
                isActive = value;
                OnIsActiveValueChanged?.Invoke(oldValue, isActive);
            }
        }

        private void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (IsActive)
            {
                foreach (var fogObject in fogWallGameObjects)
                {
                    fogObject.SetActive(true);
                }
            }
            else
            {
                foreach (var fogObject in fogWallGameObjects)
                {
                    fogObject.SetActive(false);
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();

            fogWallAudioSource = GetComponent<AudioSource>();
        }

        protected override void Start()
        {
            base.Start();

            OnIsActiveChanged(false, IsActive);
            OnIsActiveValueChanged += OnIsActiveChanged;

            WorldObjectManager.instance.AddFogWallToList(this);
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            // Face the fog wall
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward);
            player.transform.rotation = targetRotation;

            // Disable Collisions with the fog wall
            StartCoroutine(DisableCollisionForTime(player));

            // Play Sound
            fogWallAudioSource.PlayOneShot(fogWallSFX, 1f);

            // Walk through the fog wall
            player.playerAnimatorManager.PlayTargetActionAnimation("Pass_Through_Fog_01", true);

            // Reenable Collisions with fog wall
        }

        private void OnDestroy()
        {
            OnIsActiveValueChanged -= OnIsActiveChanged;
            WorldObjectManager.instance.RemoveFogWallFromList(this);
        }

        private IEnumerator DisableCollisionForTime(PlayerManager player)
        {
            // MAKE THIS FUNCTION THE SAME TIME AS THE WALKING THROUGH FOG WALL ANIMATION LENGTH
            Physics.IgnoreCollision(player.characterController, fogWallCollider, true);

            yield return new WaitForSeconds(3f);

            Physics.IgnoreCollision(player.characterController, fogWallCollider, false);
        }
    }
}
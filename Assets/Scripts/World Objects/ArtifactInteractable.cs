using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Group1
{
    public class ArtifactInteractable : Interactable
    {
        [Header("Scene")]
        [SerializeField] private int buildIndexToTravelTo = 1;

        [Header("Rotation")]
        [SerializeField] GameObject objectToRotate;
        private float acceleration = 1000f;
        [SerializeField] Vector3 spinAxis = new Vector3(1, 0.5f, 0.2f);
        private float currentSpeed = 0f;
        private bool beginRotation = false;

        [Header("SFX")]
        [SerializeField] private AudioClip artifactInteractableActivationSound;

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            // PLAY UNSTABLE ANIMATION
            beginRotation = true;

            // Start Audio
            player.characterSoundFXManager.PlaySoundFX(artifactInteractableActivationSound, 1, false);

            StartCoroutine(TeleportPlayer(5f));
        }

        void Update()
        {
            if (beginRotation)
            {
                UncontrolledSpin();
            }
        }

        private IEnumerator TeleportPlayer(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (buildIndexToTravelTo <= 1 || buildIndexToTravelTo >= SceneManager.sceneCountInBuildSettings)
            {
                WorldSaveGameManager.instance.SaveAndQuit();
            }
            else
            {
                WorldSaveGameManager.instance.LoadNewScene(buildIndexToTravelTo);
            }
        }

        private void UncontrolledSpin()
        {
            if (objectToRotate != null)
            {
                currentSpeed += acceleration * Time.deltaTime;

                objectToRotate.transform.Rotate(spinAxis * currentSpeed * Time.deltaTime);
            }
        }
    }
}
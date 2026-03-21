using UnityEngine;

namespace Group1
{
    public class CharacterFootStepSFXMaker : MonoBehaviour
    {
        CharacterManager character;

        private bool hasTouchedGround = false;
        private bool hasPlayedFootStepSFX = false;

        private void Awake()
        {
            character = GetComponentInParent<CharacterManager>();
        }

        private void FixedUpdate()
        {
            CheckForFootSteps();
        }

        private void CheckForFootSteps()
        {
            if (character == null) return;

            if (!character.IsMoving) return;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, character.transform.TransformDirection(Vector3.down), out hit, 0.05f, WorldUtilityManager.Instance.GetEnviroLayers()))
            {
                hasTouchedGround = true;
                hasPlayedFootStepSFX = false;

                if (hasPlayedFootStepSFX)
                {
                    hasTouchedGround = false;
                }
            }

            if (hasTouchedGround && !hasPlayedFootStepSFX)
            {
                hasPlayedFootStepSFX = true;

                PlayFootStepSoundFX();
            }
        }

        private void PlayFootStepSoundFX()
        {
            character.characterSoundFXManager.PlayFootStepSoundFX();
        }
    }
}
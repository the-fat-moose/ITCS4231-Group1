using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Group1
{
    public class ElevatorInteractable : Interactable
    {
        [Header("Position")]
        public Vector3 position = Vector3.zero;
        public bool elevatorIsRising = false;
        public bool elevatorIsDescending = false;
        [SerializeField] private float positionSmoothTime = 0.1f;
        [SerializeField] private float yMovementOffset = 0.125f;

        [Header("Destination")]
        [SerializeField] private float moveSpeed = 2f;
        public Vector3 destinationHigh; // WHERE THE ELEVATOR STOPS IF ITS RISING
        public Vector3 destinationLow;  // WHERE THE ELEVATOR STOPS IF ITS DESCENDING

        [Header("Recall Locations")]
        [SerializeField] private CallElevatorInteractable destinationLowRecall;
        [SerializeField] private CallElevatorInteractable destinationHighRecall;

        [Header("Characters On Elevator")]
        [SerializeField] protected List<CharacterManager> charactersOnElevator = new List<CharacterManager>();

        [Header("SFX")]
        private AudioSource elevatorAudioSource;
        [SerializeField] private AudioClip elevatorMovingSFX;
        [SerializeField] private AudioClip[] elevatorStoppingSFX;

        protected override void Awake()
        {
            base.Awake();

            elevatorAudioSource = GetComponent<AudioSource>();
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            InitializeElevatorDirection();
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (elevatorIsRising || elevatorIsDescending) return;

            base.OnTriggerEnter(other);
        }

        protected override void Start()
        {
            base.Start();

            position = transform.localPosition;

            if (elevatorIsRising) ActivateElevator(true);
            if (elevatorIsDescending) ActivateElevator(false);
        }

        private void ActivateElevator(bool isRising)
        {
            StartCoroutine(MoveElevatorCoroutine(isRising));
        }

        private IEnumerator MoveElevatorCoroutine(bool isRising)
        {
            // DISABLE THE INTERACTABLE COLLISION
            interactableCollider.enabled = false;

            // WHEN THE ELEVATOR STARTS, REMOVE IT AS AN INTERACTABLE WHILST ITS GOING
            for (int i = 0; i < charactersOnElevator.Count; i++)
            {
                if (charactersOnElevator[i] == null) continue;

                PlayerManager player = charactersOnElevator[i] as PlayerManager;

                if (player == null) continue;

                player.playerInteractionManager.RemoveInteractionFromList(this);
            }

            // MOVEMENT SFX
            elevatorAudioSource.clip = elevatorMovingSFX;
            elevatorAudioSource.Play();

            // SET THE DESTINATION
            Vector3 destination = destinationHigh;

            if (!isRising) destination = destinationLow;

            // REMOVE THE RECALL INTERACTION FROM PLAYERS
            destinationLowRecall.RemoveInteractionFromPlayers();
            destinationHighRecall.RemoveInteractionFromPlayers();

            // MOVE THE ELEVATOR
            while (transform.localPosition != destination)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, moveSpeed * Time.deltaTime);
                Vector3 velocityOfMovement = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

                position = transform.localPosition;

                for (int i = 0; i < charactersOnElevator.Count; i++)
                {
                    if (charactersOnElevator[i] == null) continue;

                    if (!charactersOnElevator[i].gameObject.activeInHierarchy) RemoveCharacterFromListOfCharactersOnElevator(charactersOnElevator[i]);

                    if (!charactersOnElevator[i].isJumping) charactersOnElevator[i].transform.position = new Vector3(charactersOnElevator[i].transform.position.x, velocityOfMovement.y + yMovementOffset, charactersOnElevator[i].transform.position.z);
                }

                yield return null;
            }

            // STOP THE MOVEMENT FLAGS
            elevatorIsRising = false;
            elevatorIsDescending = false;

            // HARD SET THE POSITION
            transform.position = destination;
            transform.localPosition = destination;
            position = destination;

            // ADD THE RECALL INTERACTION TO PLAYERS
            destinationLowRecall.AddInteractionToPlayers();
            destinationHighRecall.AddInteractionToPlayers();

            // STOP THE MOVEMENT SFX
            elevatorAudioSource.Stop();
            // PLAY STOPPING SFX
            elevatorAudioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(elevatorStoppingSFX));

            // RE-ENABLE THE INTERACTABLE COLLISION
            interactableCollider.enabled = true;

            yield return null; 
        }

        public void AddCharacterToListOfCharactersOnElevator(CharacterManager character)
        {
            if (charactersOnElevator.Contains(character)) return;

            charactersOnElevator.Add(character);
            character.characterLocomotionManager.isRidingLift = true;
        }

        public void RemoveCharacterFromListOfCharactersOnElevator(CharacterManager character)
        {
            if (!charactersOnElevator.Contains(character)) return;

            charactersOnElevator.Remove(character);
            character.characterLocomotionManager.isRidingLift = false;
        }
    
        public void InitializeElevatorDirection()
        {
            if (transform.localPosition == destinationHigh)
            {
                elevatorIsDescending = true;

                ActivateElevator(false);
            }
            else if (transform.localPosition == destinationLow)
            {
                elevatorIsRising = true;

                ActivateElevator(true);
            }
        }
    }
}
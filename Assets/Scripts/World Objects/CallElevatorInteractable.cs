using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Group1
{
    public class CallElevatorInteractable : Interactable
    {
        [Header("Elevator")]
        [SerializeField] ElevatorInteractable elevator;

        [Header("Players within Interaction Radius")]
        public List<PlayerManager> playersWithinInteractionTrigger = new List<PlayerManager>();

        [Header("Top/Bottom Call")]
        [SerializeField] private bool isTopDestination = true;

        [Header("Model Effects")]
        public GameObject model;
        [SerializeField] private Material unactivatedMaterial;
        public Material accessibleMaterial;
        public Material inaccessibleMaterial;

        private Coroutine waitForElevatorTravelCoroutine;

        protected override void Start()
        {
            base.Start();

            model.GetComponent<Renderer>().material = unactivatedMaterial;
        }

        public override void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null) AddPlayerToListOfPlayersInInteractionTrigger(player);
        }

        public override void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null) RemovePlayerFromListOfPlayersInInteractionTrigger(player);
        }

        public override void Interact(PlayerManager player)
        {
            elevator.InitializeElevatorDirection();
        }

        private void AddPlayerToListOfPlayersInInteractionTrigger(PlayerManager player)
        {
            // CHECK FOR NULLS INCASE SOMEBODY ON THE ELEVATOR DISCONNECTS DURING INTERACTION RADIUS CHECK
            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null) playersWithinInteractionTrigger.RemoveAt(i);
            }

            if (playersWithinInteractionTrigger.Contains(player)) return;

            playersWithinInteractionTrigger.Add(player);

            if (waitForElevatorTravelCoroutine != null) StopCoroutine(waitForElevatorTravelCoroutine);

            waitForElevatorTravelCoroutine = StartCoroutine(CheckForCharactersInTrigger());
        }

        private void RemovePlayerFromListOfPlayersInInteractionTrigger(PlayerManager player)
        {
            if (!playersWithinInteractionTrigger.Contains(player)) return;

            player.playerInteractionManager.RemoveInteractionFromList(this);
            playersWithinInteractionTrigger.Remove(player);

            // CHECK FOR NULLS INCASE SOMEBODY ON THE ELEVATOR DISCONNECTS DURING INTERACTION RADIUS CHECK
            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null) playersWithinInteractionTrigger.RemoveAt(i);
            }
        }

        private IEnumerator CheckForCharactersInTrigger()
        {
            while (elevator.elevatorIsRising || elevator.elevatorIsDescending) yield return null;

            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null) continue;

                if (isTopDestination && elevator.position == elevator.destinationLow)
                    playersWithinInteractionTrigger[i].playerInteractionManager.AddInteractionToList(this);

                if (!isTopDestination && elevator.position == elevator.destinationHigh)
                    playersWithinInteractionTrigger[i].playerInteractionManager.AddInteractionToList(this);
            }
        }
    
        public void RemoveInteractionFromPlayers()
        {
            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null) continue;

                playersWithinInteractionTrigger[i].playerInteractionManager.RemoveInteractionFromList(this);
            }
        }

        public void AddInteractionToPlayers()
        {
            for(int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null) continue;

                if (isTopDestination && elevator.position == elevator.destinationLow)
                        playersWithinInteractionTrigger[i].playerInteractionManager.AddInteractionToList(this);

                if (!isTopDestination && elevator.position == elevator.destinationHigh)
                    playersWithinInteractionTrigger[i].playerInteractionManager.AddInteractionToList(this);
            }
        }
    }
}
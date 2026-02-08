using UnityEngine;

namespace Group1{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        private void OnEnable()
        {
            character.OnIsChargingAttackChanged += HandleChargingChanged;
        }

        private void OnDisable()
        {
            character.OnIsChargingAttackChanged -= HandleChargingChanged;
        }

        private void HandleChargingChanged(bool oldValue, bool newValue)
        {
            player.animator.SetBool("isChargingAttack", newValue);
        }

        public void OnAnimatorMove()
        {
            if (player.applyRootMotion)
            {
                Vector3 velocity = player.animator.deltaPosition;
                player.characterController.Move(velocity);
                player.transform.rotation *= player.animator.deltaRotation;
            }
        }
    }
}

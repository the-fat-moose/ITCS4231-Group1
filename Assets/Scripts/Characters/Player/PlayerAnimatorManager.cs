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

        public void OnPushAbilityHit()
        {
            if (player.playerInventoryManager.pushAbility is PushAbility push)
            {
                push.ExecutePush(player);
            }
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
    
        public override void EnableCanDoCombo()
        {
            if(player.CurrentWeaponBeingUsed != 0)
            {
                player.playerCombatManager.canComboWithMainHandWeapon = true;
            }
        }

        public override void DisableCanDoCombo()
        {
            
            player.playerCombatManager.canComboWithMainHandWeapon = false;

        }

        public void EnableInvulnerability()
        {
            player.isInvulnerable = true;
            Debug.LogError("Player is now invulnerable");
        }

        public void DisableInvulnerability()
        {
            player.isInvulnerable = false;
        }
    
    }
}

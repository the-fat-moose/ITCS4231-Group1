using UnityEngine;

namespace Group1 {
    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider meleeDamageCollider;

        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon, bool rtsrEnabled = false)
        {
            meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
            
            if (rtsrEnabled)
                meleeDamageCollider.physicalDamage = weapon.physicalDamage * 1.2f;
            else
                meleeDamageCollider.physicalDamage = weapon.physicalDamage;
            meleeDamageCollider.magicDamage = weapon.magicDamage;

            meleeDamageCollider.light_Attack_Modifier = weapon.light_Attack_Modifier;
            meleeDamageCollider.heavy_Attack_Modifier = weapon.heavy_Attack_Modifier;
        }
    }
}
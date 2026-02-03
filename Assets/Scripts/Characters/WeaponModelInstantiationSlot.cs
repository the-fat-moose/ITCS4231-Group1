using UnityEngine;

namespace Group1 {
    public class WeaponModelInstantiationSlot : MonoBehaviour
    {
        public WeaponModelSlot weaponSlot;
        public GameObject currentWeaponModel;

        public Vector3 weaponScale;

        public void UnloadWeapon()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeapon(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            if (weaponScale != Vector3.zero)
            {
                weaponModel.transform.localScale = weaponScale;
            }
            else
            {
                weaponModel.transform.localScale = Vector3.one;
            }
        }
    }
}
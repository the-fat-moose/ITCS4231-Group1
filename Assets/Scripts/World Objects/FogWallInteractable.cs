using UnityEngine;

namespace Group1 {
    public class FogWallInteractable : MonoBehaviour
    {
        [Header("Fog")]
        [SerializeField] private GameObject[] fogWallGameObjects;

        [Header("I.D")]
        public int fogWallID;

        [Header("Active")]
        private bool isActive = true;

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

        private void Start()
        {
            OnIsActiveChanged(false, IsActive);
            OnIsActiveValueChanged += OnIsActiveChanged;

            WorldObjectManager.instance.AddFogWallToList(this);
        }

        private void OnDestroy()
        {
            OnIsActiveValueChanged -= OnIsActiveChanged;
            WorldObjectManager.instance.RemoveFogWallFromList(this);
        }
    }
}
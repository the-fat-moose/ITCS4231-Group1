using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Group1 {
    public class UI_Match_Scroll_Wheel_To_Selected_Button : MonoBehaviour
    {
        [SerializeField] private GameObject currentSelected;
        [SerializeField] private GameObject previouslySelected;
        [SerializeField] private RectTransform currentlySelectedTransform;

        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private ScrollRect scrollRect;

        private void Update()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected != null)
            {
                if (currentSelected != previouslySelected)
                {
                    previouslySelected = currentSelected;
                    currentlySelectedTransform = currentSelected.GetComponent<RectTransform>();
                    SnapTo(currentlySelectedTransform);
                }
            }
        }

        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            Vector2 newPosition = 
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

            // WE ONLY WANT THE Y
            newPosition.x = 0;
            
            contentPanel.anchoredPosition = newPosition;
        }
    }
}
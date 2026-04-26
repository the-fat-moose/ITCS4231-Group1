using UnityEngine;
using UnityEngine.SceneManagement;

namespace Group1
{
    public class StarWarsTextCrawl : MonoBehaviour
    {
        [SerializeField] private float speed = 50f;
        RectTransform rt;
        [SerializeField] private Vector2 endAnchorPoint;

        private void Start()
        {
            rt = GetComponent<RectTransform>();
        }

        private void Update()
        {
            rt.anchoredPosition += Vector2.up * speed * Time.deltaTime;

            if (rt.anchoredPosition.y >= endAnchorPoint.y)
            {
                SceneManager.LoadScene(1); // Main Menu
            }
        }
    }
}
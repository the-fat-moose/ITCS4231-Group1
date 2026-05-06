using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    public Material scrollTexture;
    public float scrollSpeedX = 0.1f;
    public float scrollSpeedY = 0.05f;

    private Vector2 currentOffset;

    void Update()
    {
        currentOffset.x += scrollSpeedX * Time.deltaTime;
        currentOffset.y += scrollSpeedY * Time.deltaTime;

        scrollTexture.SetTextureOffset("_BaseMap", currentOffset);
    }
}

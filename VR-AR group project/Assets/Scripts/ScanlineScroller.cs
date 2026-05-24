using UnityEngine;
using UnityEngine.UI;

public class ScanlineScroller : MonoBehaviour
{
    public RawImage scanlineImage;
    public float scrollSpeed = 0.5f;

    void Start()
    {
        scanlineImage = GetComponent<RawImage>();
    }

    void Update()
    {
        if (scanlineImage)
        {
            Vector2 offset = scanlineImage.uvRect.position;
            offset.y += scrollSpeed * Time.deltaTime;

            if (offset.y > 1f) offset.y -= 1f;

            scanlineImage.uvRect = new Rect(offset, scanlineImage.uvRect.size);
        }
    }
}
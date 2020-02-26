using UnityEngine;

public class SpriteOffsetController : MonoBehaviour
{
    public Vector2 scrollDir;
    public float scrollSpeed;

    private SpriteRenderer spriteRenderer;
    private Vector2 curOffset;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        curOffset = spriteRenderer.material.GetVector("_Offset");
        spriteRenderer.material.SetVector("_Offset", curOffset - (scrollDir * scrollSpeed * Time.deltaTime));
    }
}

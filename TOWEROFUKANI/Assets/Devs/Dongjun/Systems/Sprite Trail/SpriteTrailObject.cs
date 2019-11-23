using UnityEngine;

public class SpriteTrailObject : SelfSleepObj
{
    [SerializeField] private float fadeSpeed;

    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected virtual void Update()
    {
        Color fadedColor = spriteRenderer.color;
        fadedColor.a -= fadeSpeed * Time.deltaTime;
        spriteRenderer.color = fadedColor;
    }

    public void SetSprite(SpriteRenderer refSpriteRenderer)
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        spriteRenderer.sprite = refSpriteRenderer.sprite;
        spriteRenderer.flipX = refSpriteRenderer.flipX;
        spriteRenderer.flipY = refSpriteRenderer.flipY;
    }
}

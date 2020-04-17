using UnityEngine;

public class Tile : PoolingObj
{
    public Sprite icon;
    public Vector2Int size = new Vector2Int(1, 1);

    public override void OnSpawn()
    {

    }

    public void Init()
    {
        if (icon == null)
            icon = GetComponentInChildren<SpriteRenderer>().sprite;

        if (size.x < 1) size.x = 1;
        if (size.y < 1) size.y = 1;
    }

    public void UpdateVisual()
    {

    }
}

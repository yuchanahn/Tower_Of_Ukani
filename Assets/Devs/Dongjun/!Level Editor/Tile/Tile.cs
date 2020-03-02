using UnityEngine;
using UnityEngine.UI;

public class Tile : PoolingObj
{
    public Sprite icon;

    public override void ResetOnSpawn()
    {

    }

    public void Init()
    {
        if (icon == null)
            icon = GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public void UpdateVisual()
    {

    }
}

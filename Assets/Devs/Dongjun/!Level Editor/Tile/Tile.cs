using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Sprite icon;

    public void Init()
    {
        if (icon == null)
            icon = GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public void UpdateVisual()
    {

    }
}

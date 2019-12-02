using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : ItemSlot
{
    public override void SetData(Item item)
    {
        if (item == null)
        {
            itemIcon.sprite = emptySprite;
            return;
        }

        itemIcon.sprite = item.Info.Icon;
    }
}

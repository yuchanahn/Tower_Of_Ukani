using UnityEngine;

public class WeaponItemSlot : ItemSlot
{
    public override void SetData(Item item)
    {
        if (item == null)
        {
            itemIcon.sprite = emptySprite;
            return;
        }

        if (item is WeaponItem)
        {
            itemIcon.sprite = item.Info.Icon;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PassiveItemSlot : ItemSlot
{
    [SerializeField] private Text itemName;

    public override void SetData(Item item)
    {
        if (item is PassiveItem)
        {
            itemIcon.sprite = item.Info.Icon;
            itemName.text = $"{item.Info.Name} x{item.Count}";
        }
    }
}

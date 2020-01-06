using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonStackableItemUI : DraggableItemUI
{
    [Header("Item")]
    [SerializeField] private Image iconImage;

    public override void OnUpdateUI(Item item)
    {
        base.OnUpdateUI(item);
        iconImage.sprite = item.Info.Icon;
    }

    public override void ShowTooltip()
    {
    }
    public override void HideTooltip()
    {
    }
    public override void UpdateTooltip()
    {
    }
}

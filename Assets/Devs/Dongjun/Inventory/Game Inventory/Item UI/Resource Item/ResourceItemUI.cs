using UnityEngine;
using UnityEngine.UI;

public class ResourceItemUI : DraggableItemUI
{
    [Header("Item")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Text countText;

    public override void OnUpdateUI(Item item)
    {
        base.OnUpdateUI(item);
        iconImage.sprite = item.Info.Icon;
        countText.text = item.Info.Count.ToString();
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

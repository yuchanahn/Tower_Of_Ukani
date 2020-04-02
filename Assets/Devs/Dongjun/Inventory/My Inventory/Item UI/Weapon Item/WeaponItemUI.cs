using UnityEngine;
using UnityEngine.UI;

public class WeaponItemUI : DraggableItemUI
{
    [Header("Item")]
    [SerializeField] private Image iconImage;

    public override void OnUpdateUI(Item item)
    {
        base.OnUpdateUI(item);
        iconImage.sprite = item.Info.Icon;

        float imgSize = Mathf.Max(iconImage.sprite.rect.width, iconImage.sprite.rect.height) / 16f;
        iconImage.GetComponent<RectTransform>().localScale = new Vector3(imgSize, imgSize, 1);
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

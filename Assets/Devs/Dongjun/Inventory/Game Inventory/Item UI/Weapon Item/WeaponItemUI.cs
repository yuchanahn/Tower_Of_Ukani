using UnityEngine;
using UnityEngine.UI;

public class WeaponItemUI : DraggableItemUI
{
    // TODO:
    // 툴팁 만들기.
    // 툴팁에 아이템 능력 설명 포함.

    [Header("Item")]
    [SerializeField] private Image iconImage;

    //[Header("Tooltip")]
    //[SerializeField] private WeaponTooltipUI tooltipPrefab;
    //
    //private BasicTooltipUI tooltipObject;

    public override void OnUpdateUI(Item item)
    {
        base.OnUpdateUI(item);
        
        // Set Icon
        iconImage.sprite = item.Info.Icon;
        // Set Icon Size
        float imgSize = Mathf.Max(iconImage.sprite.rect.width, iconImage.sprite.rect.height) / 16f;
        iconImage.GetComponent<RectTransform>().localScale = new Vector3(imgSize, imgSize, 1);
    }

    public override void ShowTooltip()
    {
        //if (tooltipObject == null)
        //    return;

        //tooltipObject.Show();
        //UpdateTooltip();
    }
    public override void HideTooltip()
    {
        //if (tooltipObject == null)
        //    return;

        //tooltipObject.Hide();
    }
    public override void UpdateTooltip()
    {
        //if (tooltipObject == null)
        //    return;

        //tooltipObject.UpdateTooltip(Item);
    }
}

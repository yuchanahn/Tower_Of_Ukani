using UnityEngine;
using UnityEngine.UI;

public class ActiveItemUI : DraggableItemUI
{
    [Header("Item")]
    [SerializeField] private Image iconImage;

    [Header("Active Indicator")]
    [SerializeField] private GameObject activeIndicator;

    [Header("Cooldown Indicator")]
    [SerializeField] private GameObject cooldownIndicator;
    [SerializeField] private UI_Fill cooldownFill;
    [SerializeField] private Text cooldownTime;

    private void LateUpdate()
    {
        ActiveItem activeItem = Item as ActiveItem;

        // Active Indicator
        activeIndicator.SetActive(activeItem.IsActive);

        // Cooldown Indicator
        if (!activeItem.cooldownTimer.IsEnded && !activeItem.IsActive)
        {
            cooldownIndicator.SetActive(true);
            cooldownFill.Value = 1 - (activeItem.cooldownTimer.CurTime / activeItem.cooldownTimer.EndTime);

            float cooldown = activeItem.cooldownTimer.EndTime - activeItem.cooldownTimer.CurTime;
            cooldownTime.text = (cooldown >= 2) ? cooldown.ToString("0") : cooldown.ToString("0.0");
        }
        else
        {
            cooldownIndicator.SetActive(false);
        }
    }

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

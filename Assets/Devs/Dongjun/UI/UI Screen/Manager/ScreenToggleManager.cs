using DG.Tweening;
using Dongjun.Helper;
using UnityEngine;

public class ScreenToggleManager : MonoBehaviour
{
    [Header("Closeable UI")]
    [SerializeField] private UI_Screen inventory;

    [Header("Active Hotbar")]
    [SerializeField] private GameObject activeHotbar;
    [SerializeField] private RectTransform activeHotbarTitle;
    [SerializeField] private RectTransform activeHotbarSlots;

    [Header("Consumable Hotbar")]
    [SerializeField] private GameObject consumableHotbar;
    [SerializeField] private RectTransform consumableHotbarTitle;
    [SerializeField] private RectTransform consumableHotbarSlots;

    void Update()
    {
        if (Input.GetKeyDown(PlayerUIKeys.InventoryToggle))
            inventory.Toggle();

        if (Input.GetKeyDown(PlayerUIKeys.ActiveHotbarToggle))
        {
            void AnimateHotbar(RectTransform title, RectTransform slots)
            {
                title.anchoredPosition = consumableHotbarTitle.anchoredPosition.Change(x: -80);
                title.DOAnchorPosX(2, 0.25f).SetEase(Ease.OutCubic);

                slots.anchoredPosition = consumableHotbarSlots.anchoredPosition.Change(y: -20);
                slots.DOAnchorPosY(0, 0.3f).SetEase(Ease.OutCubic);
            }

            if (activeHotbar.gameObject.activeSelf)
            {
                activeHotbar.SetActive(false);
                consumableHotbar.SetActive(true);
                AnimateHotbar(consumableHotbarTitle, consumableHotbarSlots);
            }
            else
            {
                activeHotbar.SetActive(true);
                consumableHotbar.SetActive(false);
                AnimateHotbar(activeHotbarTitle, activeHotbarSlots);
            }
        }
    }
}

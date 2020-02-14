using DG.Tweening;
using UnityEngine;

public class ScreenToggleManager : MonoBehaviour
{
    [Header("Closeable UI")]
    [SerializeField] private UI_Screen inventory;

    [Header("Swap UI")]
    [SerializeField] private RectTransform activeHotbar;
    [SerializeField] private RectTransform consumableHotbar;

    void Update()
    {
        if (Input.GetKeyDown(PlayerUIKeys.InventoryToggle))
            inventory.Toggle();

        if (Input.GetKeyDown(PlayerUIKeys.ActiveHotbarToggle))
        {
            bool activeActive = activeHotbar.gameObject.activeSelf;

            if (activeActive)
            {
                activeHotbar.gameObject.SetActive(false);
                consumableHotbar.gameObject.SetActive(true);

                consumableHotbar.anchoredPosition = new Vector2(0, -20);
                consumableHotbar.DOAnchorPosY(0, 0.3f).SetEase(Ease.OutCubic);
            }
            else
            {
                consumableHotbar.gameObject.SetActive(false);
                activeHotbar.gameObject.SetActive(true);

                activeHotbar.anchoredPosition = new Vector2(0, -20);
                activeHotbar.DOAnchorPosY(0, 0.3f).SetEase(Ease.OutCubic);
            }
        }
    }
}

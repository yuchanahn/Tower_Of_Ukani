using UnityEngine;

public class WeaponHotbarSlot : DroppableSlotUI
{
    [SerializeField] private GameObject selectedIndicator;

    public void Select(bool isSelected)
    {
        selectedIndicator.SetActive(isSelected);
    }
}

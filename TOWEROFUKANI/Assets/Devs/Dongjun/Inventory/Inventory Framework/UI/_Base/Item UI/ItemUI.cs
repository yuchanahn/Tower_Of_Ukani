using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ItemUI : MonoBehaviour,
    IPointerDownHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    #region Var: Refs
    private ItemRightClickAction rightCickManager;
    #endregion

    #region Var: Properties
    public Item Item
    { get; private set; }
    #endregion

    #region Method: Initialize
    public void Init(string name, Transform parent, ItemRightClickAction rightClickManager)
    {
        gameObject.name = name;
        transform.SetParent(parent, false);
        SetRightClickManager(rightClickManager);
    }
    #endregion

    #region Method: Right Click
    public void SetRightClickManager(ItemRightClickAction rightCickManager)
    {
        this.rightCickManager = rightCickManager;
    }
    #endregion

    #region Method: Update UI
    public virtual void OnUpdateUI(Item item)
    {
        gameObject.name = item.Info.ItemName;
        Item = item;
    }
    #endregion

    #region Method: Tooltip
    public abstract void ShowTooltip();
    public abstract void HideTooltip();
    public abstract void UpdateTooltip();
    #endregion

    #region Interface: Unity UI Event
    protected virtual void OnPointerDown(PointerEventData eventData)
    {
        if (rightCickManager != null && eventData.button != PointerEventData.InputButton.Right)
            HideTooltip();

        if (Input.GetKey(KeyCode.Mouse0) || eventData.button != PointerEventData.InputButton.Right)
            return;

        rightCickManager?.OnRightClick(Item);
    }
    protected virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.Mouse0))
            return;

        ShowTooltip();
    }
    protected virtual void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnPointerDown(eventData);
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnter(eventData);
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        OnPointerExit(eventData);
    }
    #endregion
}

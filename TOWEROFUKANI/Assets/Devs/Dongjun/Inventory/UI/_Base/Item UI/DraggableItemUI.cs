using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

// INFO!
// Exe Order = OnDrop() -> OnEndDrag()

public abstract class DraggableItemUI : ItemUI,
    IPointerUpHandler,
    IEndDragHandler,
    IDragHandler
{
    #region Var: Components
    protected Canvas thisCanvas;
    protected CanvasGroup canvasGroup;
    #endregion

    #region Var: Refs
    protected Camera mainCam;
    protected Canvas parentCanvas;
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        // Get Components
        thisCanvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Get Refs
        mainCam = Camera.main;
        parentCanvas = GetComponentInParent<Canvas>();
    }
    #endregion

    #region Method: Follow Mouse
    protected virtual void FollowMouse()
    {
        // Move To Mouse Pos
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out var pos);
        transform.position = parentCanvas.transform.TransformPoint(pos);

        // Disable Raycast Check
        canvasGroup.blocksRaycasts = false;

        // Visual
        canvasGroup.alpha = 0.6f;
        thisCanvas.overrideSorting = true;
        thisCanvas.sortingOrder = 10;
    }
    protected virtual void FollowMouseEnd()
    {
        // Reset Pos
        transform.localPosition = Vector2.zero;

        // Enable Raycast Check
        canvasGroup.blocksRaycasts = true;

        // Visual
        canvasGroup.alpha = 1f;
        thisCanvas.overrideSorting = false;

        // 갓갓 유니티!!!
        // 텍스트 흐려지는 버그: https://forum.unity.com/threads/case-1172941-fuzzy-blurry-text-after-moving-visualelement-2019-3-0a10.717584/
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
    #endregion

    #region Interface: Unity UI Event
    protected override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        FollowMouse();
    }
    protected virtual void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        FollowMouseEnd();
    }
    protected virtual void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        FollowMouse();
    }
    protected virtual void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        FollowMouseEnd();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        OnPointerUp(eventData);
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        OnEndDrag(eventData);
    }
    #endregion
}

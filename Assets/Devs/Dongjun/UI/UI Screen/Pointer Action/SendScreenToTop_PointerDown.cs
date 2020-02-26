using UnityEngine;
using UnityEngine.EventSystems;

public class SendScreenToTop_PointerDown : MonoBehaviour,
    IPointerDownHandler
{
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // Send Screen To Top
        CloseableScreenManager.Inst.SendScreenToTop(gameObject.GetComponentInParent<UI_Screen>());
    }
}

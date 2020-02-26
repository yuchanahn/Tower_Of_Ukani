using UnityEngine;
using UnityEngine.EventSystems;

public class SendScreenToTop_PointerEnter : MonoBehaviour,
    IPointerEnterHandler
{
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        // Send Screen To Top
        CloseableScreenManager.Inst.SendScreenToTop(gameObject.GetComponentInParent<UI_Screen>());
    }
}

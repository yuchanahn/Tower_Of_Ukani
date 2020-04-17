using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UI_Utility
{
    public static bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
    public static void ForceUpdate(RectTransform root)
    {
        // ※ 주의! Awake에서 작동 안됨!!!
        LayoutRebuilder.ForceRebuildLayoutImmediate(root);
    }

    public static void ClampToCanvas(RectTransform canvasRT, RectTransform targetRT, Vector2 pos)
    {
        targetRT.position = canvasRT.transform.TransformPoint(pos);

        Vector3[] canvasCorners = new Vector3[4];
        Vector3[] targetCorners = new Vector3[4];
        canvasRT.GetWorldCorners(canvasCorners);
        targetRT.GetWorldCorners(targetCorners);

        // Right
        if (targetCorners[2].x > canvasCorners[2].x)
        {
            pos.x = (canvasRT.rect.width * 0.5f) - (targetRT.rect.width * (1 - targetRT.pivot.x));
        }
        // Left
        else if (targetCorners[0].x < canvasCorners[0].x)
        {
            pos.x = (-canvasRT.rect.width * 0.5f) + (targetRT.rect.width * targetRT.pivot.x);
        }

        // Top
        if (targetCorners[2].y > canvasCorners[2].y)
        {
            pos.y = (canvasRT.rect.height * 0.5f) - (targetRT.rect.height * (1 - targetRT.pivot.y));
        }
        // Bottom
        else if (targetCorners[0].y < canvasCorners[0].y)
        {
            pos.y = (-canvasRT.rect.height * 0.5f) + (targetRT.rect.height * targetRT.pivot.y);
        }

        targetRT.position = canvasRT.transform.TransformPoint(pos);
    }
    public static void MoveTo(Canvas canvas, RectTransform targetRT, Vector2 position)
    {
        ForceUpdate(targetRT);
        RectTransform canvasRT = canvas.transform as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, position, canvas.worldCamera, out var pos);
        ClampToCanvas(canvasRT, targetRT, pos);
    }
}

using UnityEngine;

public static class LookAtMouse_Logic
{
    public static void LookAtMouseY(this Transform target, Camera mainCam, Transform pivot)
    {
        Vector3 newRot = Vector3.zero;
        newRot.y = mainCam.ScreenToWorldPoint(Input.mousePosition).x - pivot.position.x > 0 ? 0f : 180f;

        target.localRotation = Quaternion.Euler(newRot);
    }
    public static void LookAtMouseX(this Transform target, Camera mainCam, Transform pivot)
    {
        Vector3 newRot = Vector3.zero;
        newRot.x = mainCam.ScreenToWorldPoint(Input.mousePosition).x - pivot.position.x > 0 ? 0f : 180f;

        target.localRotation = Quaternion.Euler(newRot);
    }
    public static void LookAtMouse(this Transform target, Camera mainCam, Transform pivot)
    {
        target.right = (Vector2)(mainCam.ScreenToWorldPoint(Input.mousePosition) - pivot.position).normalized;

        if ((Vector2)target.right == Vector2.left)
            target.localRotation = Quaternion.Euler(0f, 0f, 180f);
    }

    public static void AimedWeapon(Camera mainCam, Transform spriteRoot, Transform pivot)
    {
        pivot.LookAtMouse(mainCam, pivot);
        spriteRoot.LookAtMouseX(mainCam, pivot);
    }
}

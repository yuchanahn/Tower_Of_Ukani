using UnityEngine;

public static class LookAtMouse_Logic
{
    public static void LookAtMouseFlipX(this SpriteRenderer target, Camera cam, Transform pivot)
    {
        target.flipX = cam.ScreenToWorldPoint(Input.mousePosition).x - pivot.position.x <= 0;
    }
    public static void LookAtMouseFlipY(this SpriteRenderer target, Camera cam, Transform pivot)
    {
        target.flipY = cam.ScreenToWorldPoint(Input.mousePosition).x - pivot.position.x <= 0;
    }

    public static void LookAtMouseFlipX(this Transform target, Camera cam, Transform pivot)
    {
        Vector3 lookRot = target.localEulerAngles;
        lookRot.y = cam.ScreenToWorldPoint(Input.mousePosition).x - pivot.position.x > 0 ? 0f : 180f;
        target.localRotation = Quaternion.Euler(lookRot);
    }
    public static void LookAtMouseFlipY(this Transform target, Camera mainCam, Transform pivot)
    {
        Vector3 lookRot = target.localEulerAngles;
        lookRot.x = mainCam.ScreenToWorldPoint(Input.mousePosition).y - pivot.position.y > 0 ? 0f : 180f;
        target.localRotation = Quaternion.Euler(lookRot);
    }

    public static void LookAtMouse(this Transform target, Camera cam, Transform pivot)
    {
        target.right = (Vector2)(cam.ScreenToWorldPoint(Input.mousePosition) - pivot.position).normalized;

        if ((Vector2)target.right == Vector2.left)
            target.localRotation = Quaternion.Euler(0f, 0f, 180f);
    }

    public static void AimMouse(this Transform target, Camera cam, Transform pivot)
    {
        // Rotate To Mouse Dir (Absolute X)
        Vector2 mouseDir = (cam.ScreenToWorldPoint(Input.mousePosition) - pivot.position).normalized;
        mouseDir.x = Mathf.Abs(mouseDir.x);
        target.right = mouseDir;

        // Flip X
        target.LookAtMouseFlipX(cam, pivot);
    }

    public static void AimMouse(this GameObject target, Camera cam, Transform pivot)
    {
        // Rotate To Mouse Dir (Absolute X)
        Vector2 mouseDir = (cam.ScreenToWorldPoint(Input.mousePosition) - pivot.position).normalized;
        mouseDir.x = Mathf.Abs(mouseDir.x);
        target.transform.right = mouseDir;

        // Flip X
        target.transform.LookAtMouseFlipX(cam, pivot);
    }

    public static Vector2 GetMouseDir(Camera cam, Vector2 pivot)
    {
        return (cam.ScreenToWorldPoint(Input.mousePosition) - (Vector3)pivot).normalized;
    }
}

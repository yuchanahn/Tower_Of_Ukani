using UnityEngine;

public class LookAtMouse_Logic
{
    public static void FlipY(Camera mainCam, Transform tf_Target, Transform tf_Pivot)
    {
        Vector3 newRot = Vector3.zero;
        newRot.y = mainCam.ScreenToWorldPoint(Input.mousePosition).x - tf_Pivot.position.x > 0 ? 0f : 180f;

        tf_Target.localRotation = Quaternion.Euler(newRot);
    }
    public static void FlipX(Camera mainCam, Transform tf_Target, Transform tf_Pivot)
    {
        Vector3 newRot = Vector3.zero;
        newRot.x = mainCam.ScreenToWorldPoint(Input.mousePosition).x - tf_Pivot.position.x > 0 ? 0f : 180f;

        tf_Target.localRotation = Quaternion.Euler(newRot);
    }
    public static void Rotate(Camera mainCam, Transform tf_Target, Transform tf_Pivot)
    {
        tf_Target.right = (Vector2)(mainCam.ScreenToWorldPoint(Input.mousePosition) - tf_Pivot.position).normalized;

        if ((Vector2)tf_Target.right == Vector2.left)
            tf_Target.localRotation = Quaternion.Euler(0f, 0f, 180f);
    }
}

using UnityEngine;

public static class Flip_Logic
{
    public static void FlipXTo(int dir, Transform tf)
    {
        Vector3 rot = tf.localEulerAngles;
        rot.y = dir == 1 ? 0f : 180f;
        tf.localRotation = Quaternion.Euler(rot);
    }
    public static void FlipYTo(int dir, Transform tf)
    {
        Vector3 rot = tf.localEulerAngles;
        rot.x = dir == 1 ? 0f : 180f;
        tf.localRotation = Quaternion.Euler(rot);
    }
}

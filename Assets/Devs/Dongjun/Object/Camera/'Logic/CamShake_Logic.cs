using UnityEngine;

public static class CamShake_Logic
{
    private static float Angle360(Vector3 from, Vector3 to, Vector3 right)
    {
        float angle = Vector3.Angle(from, to);
        return (Vector3.Angle(right, to) > 90f) ? 360f - angle : angle;
    }

    public static void ShakeRandom(CameraShake.Data camShakeData)
    {
        camShakeData.angle = Random.Range(0f, 360f);
        CamManager.Inst.CamShake.StartShake(camShakeData);
    }
    public static void ShakeDir(CameraShake.Data camShakeData, Transform tf, Vector2 dir)
    {
        camShakeData.angle = Angle360(tf.right, dir, Vector3.right);
        CamManager.Inst.CamShake.StartShake(camShakeData);
    }
}

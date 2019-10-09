using UnityEngine;

public static class CamShake_Logic
{
    public static void ShakeRandom(CameraShake.Data camShakeData)
    {
        camShakeData.angle = Random.Range(0f, 360f);
        Global.Inst.CamShake.StartShake(camShakeData);
    }
    public static void ShakeBackward(CameraShake.Data camShakeData, Transform tf)
    {
        camShakeData.angle = tf.eulerAngles.z - 180f;
        Global.Inst.CamShake.StartShake(camShakeData);
    }
    public static void ShakeForward(CameraShake.Data camShakeData, Transform tf)
    {
        camShakeData.angle = tf.eulerAngles.z;
        Global.Inst.CamShake.StartShake(camShakeData);
    }
}

using UnityEngine;

public static class CamShake_Logic
{
    public static void ShakeBackward(CameraShake.Data camShakeData, Transform tf)
    {
        camShakeData.angle = tf.eulerAngles.z - 180f;
        CommonObjs.Inst.CamShake.StartShake(camShakeData);
    }
    public static void ShakeForward(CameraShake.Data camShakeData, Transform tf)
    {
        camShakeData.angle = tf.eulerAngles.z;
        CommonObjs.Inst.CamShake.StartShake(camShakeData);
    }
}

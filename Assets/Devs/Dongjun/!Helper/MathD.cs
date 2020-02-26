using UnityEngine;

public static class MathD
{
    public static int Round(float f)
    {
        return (int)(f + 0.5f);
    }

    public static int SetSign(int target, int sign)
    {
        if (Mathf.Sign(target) == Mathf.Sign(sign))
            return target;
        else
            return -target;
    }
    public static float SetSign(float target, float sign)
    {
        if (Mathf.Sign(target) == Mathf.Sign(sign))
            return target;
        else
            return -target;
    }
}

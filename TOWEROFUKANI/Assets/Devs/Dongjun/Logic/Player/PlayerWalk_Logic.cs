using UnityEngine;

[System.Serializable]
public struct PlayerWalkData
{
    public float walkSpeed;
    public float walkAccel;
    [Range(0, 1)] public float changeDirSpeed;

    [HideInInspector] public float curWalkSpeed;
    [HideInInspector] public int oldWalkDir;
}

public static class PlayerWalk_Logic
{
    public static void Walk(int inputDir, Rigidbody2D rb2D, ref PlayerWalkData data, bool isJumping)
    {
        data.curWalkSpeed = (inputDir == 0) ? 0 :
                            (isJumping == true) ? data.walkSpeed :
                            (inputDir != data.oldWalkDir) ? data.curWalkSpeed * data.changeDirSpeed :
                            (data.curWalkSpeed < data.walkSpeed) ? Mathf.Lerp(0, data.walkSpeed, (data.curWalkSpeed / data.walkSpeed) + (data.walkAccel * Time.fixedDeltaTime)) :
                            data.walkSpeed;

        data.oldWalkDir = inputDir;
        rb2D.velocity = new Vector2(Mathf.Min(data.curWalkSpeed, data.walkSpeed) * inputDir, rb2D.velocity.y);
    }
}

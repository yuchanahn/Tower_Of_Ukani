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
        if (inputDir == 0)
            data.curWalkSpeed = 0;
        else if (isJumping)
            data.curWalkSpeed = data.walkSpeed;
        else if (data.oldWalkDir != inputDir)
            data.curWalkSpeed *= data.changeDirSpeed;
        else if (data.curWalkSpeed < data.walkSpeed)
            data.curWalkSpeed += data.walkAccel * Time.fixedDeltaTime;

        data.oldWalkDir = inputDir;
        rb2D.velocity = new Vector2(data.curWalkSpeed * inputDir, rb2D.velocity.y);
    }
}

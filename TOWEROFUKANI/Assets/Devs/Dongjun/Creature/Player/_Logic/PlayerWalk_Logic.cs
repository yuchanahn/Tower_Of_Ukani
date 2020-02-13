using UnityEngine;

[System.Serializable]
public struct PlayerWalkData
{
    public FloatStat walkSpeed;
    public float walkAccelTime;
    [Range(0, 1)] public float changeDirSpeed;

    [HideInInspector] public float curWalkSpeed;
    [HideInInspector] public int oldWalkDir;
    [HideInInspector] public float time;

    public PlayerWalkData(FloatStat walkSpeed, float walkAccelTime, float changeDirSpeed)
    {
        this.walkSpeed = walkSpeed;
        this.walkAccelTime = walkAccelTime;
        this.changeDirSpeed = changeDirSpeed;

        curWalkSpeed = 0;
        oldWalkDir = 0;
        time = 0;
    }
}

public static class PlayerWalk_Logic
{
    public static void Walk(this ref PlayerWalkData data, int inputDir, Rigidbody2D rb2D, bool isJumping)
    {
        if (inputDir == 0)
        {
            data.curWalkSpeed = 0;
            data.time = 0;
        }
        else if (isJumping == true)
        {
            data.curWalkSpeed = data.walkSpeed.Value;
            data.time = 0;
        }
        else if (data.oldWalkDir != 0 && data.oldWalkDir != inputDir)
        {
            data.curWalkSpeed *= data.changeDirSpeed;
            data.time = 0;
        }
        else if (data.curWalkSpeed < data.walkSpeed.Value)
        {
            data.curWalkSpeed = Mathf.Lerp(0, data.walkSpeed.Value, data.time / data.walkAccelTime);
            data.time += Time.fixedDeltaTime;
        }
        else
        {
            data.curWalkSpeed = data.walkSpeed.Value;
            data.time = 0;
        }

        data.oldWalkDir = inputDir;
        rb2D.velocity = new Vector2(Mathf.Min(data.curWalkSpeed, data.walkSpeed.Value) * inputDir, rb2D.velocity.y);
    }
}

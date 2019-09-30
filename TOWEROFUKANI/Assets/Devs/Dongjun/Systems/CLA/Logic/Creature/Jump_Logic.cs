using UnityEngine;

[System.Serializable]
public struct JumpData
{
    [HideInInspector]
    public int count_Cur;
    public int count_Max;
    public float height;
    public float time;

    [HideInInspector]
    public float apexY;

    public float jumpGravity => (2 * height) / (time * time);
    public bool canJump => count_Cur < count_Max;
}

public static class Jump_Logic
{
    public static void Jump(ref bool input_Jump, ref bool isJumping, ref JumpData jumpData, Rigidbody2D rb2D, Transform tf)
    {
        ResetJumpingState(ref isJumping, ref jumpData, rb2D, tf);

        if (!input_Jump)
            return;

        input_Jump = false;

        if (!jumpData.canJump)
            return;

        isJumping = true;
        jumpData.count_Cur++;
        jumpData.apexY = tf.position.y + jumpData.height;

        // Apply Jump Velocity
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpData.jumpGravity * jumpData.time);
    }
    public static void Jump(ref bool isJumping, ref JumpData jumpData, Rigidbody2D rb2D, Transform tf)
    {
        if (!jumpData.canJump)
            return;

        isJumping = true;
        jumpData.count_Cur++;
        jumpData.apexY = tf.position.y + jumpData.height;

        // Apply Jump Velocity
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpData.jumpGravity * jumpData.time);
    }

    public static void ResetJumpingState(ref bool isJumping, ref JumpData jumpData, Rigidbody2D rb2D, Transform tf)
    {
        if (!isJumping)
            return;

        if (!(tf.position.y >= jumpData.apexY || rb2D.velocity.y <= 0))
            return;

        // Reset Y Velocity
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
        isJumping = false;
    }
    public static void ResetJumpCount(ref JumpData jumpData)
    {
        jumpData.count_Cur = 0;
    }
}

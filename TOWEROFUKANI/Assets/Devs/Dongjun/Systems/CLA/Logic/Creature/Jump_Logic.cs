using UnityEngine;

[System.Serializable]
public struct JumpData
{
    [HideInInspector]
    public float count_Cur;
    public float count_Max;
    public float height;
    public float time;
    public float jumpGravity => (2 * height) / (time * time);

    [HideInInspector]
    public float apexY;
}

public static class Jump_Logic
{
    public static void Jump(ref bool input_Jump, ref bool isJumping, ref JumpData jumpData, Rigidbody2D rb2D, Transform tf)
    {
        if (input_Jump)
        {
            input_Jump = false;

            if (jumpData.count_Cur < jumpData.count_Max)
            {
                isJumping = true;
                jumpData.count_Cur++;
                jumpData.apexY = tf.position.y + jumpData.height;

                // Apply Jump Velocity
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpData.jumpGravity * jumpData.time);
            }
        }

        // Reset Jumping State
        if (!isJumping)
            return;

        if (tf.position.y >= jumpData.apexY || rb2D.velocity.y <= 0)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
            isJumping = false;
        }
    }
    public static void Jump(ref bool isJumping, ref JumpData jumpData, Rigidbody2D rb2D, Transform tf)
    {
        if (jumpData.count_Cur < jumpData.count_Max)
        {
            isJumping = true;
            jumpData.count_Cur++;
            jumpData.apexY = tf.position.y + jumpData.height;

            // Apply Jump Velocity
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpData.jumpGravity * jumpData.time);
        }
    }
    public static void ResetJumpingState(ref bool isJumping, ref JumpData jumpData, Rigidbody2D rb2D, Transform tf)
    {
        // Reset Jumping State
        if (!isJumping)
            return;

        if (tf.position.y >= jumpData.apexY || rb2D.velocity.y <= 0)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
            isJumping = false;
        }
    }
    public static void ResetJump(ref JumpData jumpData)
    {
        jumpData.count_Cur = 0;
    }
}

using UnityEngine;

[System.Serializable]
public struct JumpData
{
    [HideInInspector]
    public int curCount;
    public int maxCount;
    public float height;
    public float time;

    [HideInInspector]
    public float apexY;
    public bool isJumping;

    [HideInInspector]
    public float fixedYVel;

    public float jumpGravity => (2 * height) / (time * time);
    public bool canJump => curCount < maxCount;
}

public static class Jump_Logic
{
    public static void FixedJump(this ref JumpData jumpData, ref bool input_Jump, Rigidbody2D rb2D)
    {
        if (!input_Jump)
            return;

        input_Jump = false;

        jumpData.fixedYVel = jumpData.jumpGravity * jumpData.time;

        if (!jumpData.canJump)
            return;

        jumpData.isJumping = true;
        jumpData.curCount++;

        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpData.fixedYVel);
    }
    public static void FixedJumpGravity(this ref JumpData jumpData, Rigidbody2D rb2D)
    {
        if (jumpData.isJumping)
        {
            jumpData.fixedYVel -= jumpData.jumpGravity * Time.fixedDeltaTime;
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpData.fixedYVel);
            jumpData.isJumping = jumpData.fixedYVel > 0;
        }
    }

    public static void Jump(this ref JumpData jumpData, ref bool input_Jump, Rigidbody2D rb2D, Transform tf)
    {
        jumpData.ResetJumpingState(rb2D, tf);

        if (!input_Jump)
            return;

        input_Jump = false;

        if (!jumpData.canJump)
            return;

        jumpData.isJumping = true;
        jumpData.curCount++;
        jumpData.apexY = tf.position.y + jumpData.height;

        // Apply Jump Velocity
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpData.jumpGravity * jumpData.time);
    }
    public static void PlayerJump(this ref JumpData jumpData, ref bool input_Jump, Rigidbody2D rb2D, Transform tf)
    {
        jumpData.ResetJumpingState(rb2D, tf);

        if (!input_Jump)
            return;

        input_Jump = false;

        if (!jumpData.canJump)
            return;

        jumpData.isJumping = true;
        jumpData.curCount++;
        jumpData.apexY = tf.position.y + jumpData.height;

        // Apply Jump Velocity
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpData.jumpGravity * jumpData.time);

        // Trigger Item Effect
        ItemEffectManager.Trigger(PlayerActions.Jump);
    }
    public static void Jump(this ref JumpData jumpData, Rigidbody2D rb2D, Transform tf)
    {
        if (!jumpData.canJump)
            return;

        jumpData.isJumping = true;
        jumpData.curCount++;
        jumpData.apexY = tf.position.y + jumpData.height;

        // Apply Jump Velocity
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpData.jumpGravity * jumpData.time);
    }

    public static void ResetJumpingState(this ref JumpData jumpData, Rigidbody2D rb2D, Transform tf)
    {
        if (!jumpData.isJumping)
            return;

        if (!(tf.position.y >= jumpData.apexY || rb2D.velocity.y <= 0))
            return;

        if (tf.position.y > jumpData.apexY)
            tf.position = new Vector3(tf.position.x, jumpData.apexY, tf.position.z);

        rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
        jumpData.isJumping = false;
    }
    public static void ResetJumpCount(this ref JumpData jumpData)
    {
        jumpData.curCount = 0;
    }
}

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

    public float jumpGravity => (2 * height) / (time * time);
    public bool canJump => curCount < maxCount;
}

public static class Jump_Logic
{
    public static void PlayerJump(this ref JumpData jumpData, ref bool input_Jump, Rigidbody2D rb2D, Transform tf)
    {
        jumpData.ResetJumpingState(rb2D, tf);

        if (!input_Jump || !jumpData.canJump)
            return;

        input_Jump = false;
        jumpData.isJumping = true;
        jumpData.curCount++;
        jumpData.apexY = tf.position.y + jumpData.height;

        // Clamp Jump Velocity
        float jumpVel = jumpData.jumpGravity * jumpData.time;
        jumpVel = Mathf.Clamp(jumpVel, 0f, (jumpData.apexY - tf.position.y) / Time.fixedDeltaTime);

        // Apply Jump Velocity
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpVel);

        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.Jump);
    }

    public static void Jump(this ref JumpData jumpData, ref bool input_Jump, Rigidbody2D rb2D, Transform tf)
    {
        jumpData.ResetJumpingState(rb2D, tf);

        if (!input_Jump || !jumpData.canJump)
            return;

        input_Jump = false;
        jumpData.isJumping = true;
        jumpData.curCount++;
        jumpData.apexY = tf.position.y + jumpData.height;

        // Clamp Jump Velocity
        float jumpVel = jumpData.jumpGravity * jumpData.time;
        jumpVel = Mathf.Clamp(jumpVel, 0f, (jumpData.apexY - tf.position.y) / Time.fixedDeltaTime);

        // Apply Jump Velocity
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpVel);
    }

    public static void ResetJumpingState(this ref JumpData jumpData, Rigidbody2D rb2D, Transform tf)
    {
        if (!jumpData.isJumping)
            return;

        if (!(tf.position.y >= jumpData.apexY || rb2D.velocity.y <= 0))
            return;

        rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
        jumpData.isJumping = false;
    }
    public static void ResetJumpCount(this ref JumpData jumpData)
    {
        jumpData.curCount = 0;
    }
}

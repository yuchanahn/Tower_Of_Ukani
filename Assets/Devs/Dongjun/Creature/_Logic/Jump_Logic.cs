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

[System.Serializable]
public struct JumpCurveData
{
    public AnimationCurve jumpCurve;
    public int maxCount;

    [HideInInspector] public bool jumpInput;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public float jumpCurTime;
    [HideInInspector] public float jumpStartY;
    [HideInInspector] public int curCount;
    [HideInInspector] public bool jumpStarted;

    public bool CanJump => curCount < maxCount;
}

public static class Jump_Logic
{
    public static bool Jump(this ref JumpCurveData jumpData, ref bool input_Jump, Rigidbody2D rb2D, in Transform tf)
    {
        bool justJumped = false;

        // Start Jump
        if (input_Jump)
        {
            input_Jump = false;

            if (jumpData.CanJump)
            {
                // Init Jump
                jumpData.isJumping = true;
                jumpData.jumpCurTime = 0;
                jumpData.jumpStartY = tf.position.y;
                jumpData.curCount++;
                jumpData.jumpStarted = false;
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0);

                justJumped = true;
            }
        }

        // Jump Logic
        if (jumpData.isJumping)
        {
            // Reset Jump
            if ((jumpData.jumpStarted && rb2D.velocity.y <= 0) || jumpData.jumpCurve.keys[jumpData.jumpCurve.length - 1].time <= jumpData.jumpCurTime)
            {
                jumpData.isJumping = false;
                jumpData.jumpCurTime = 0;
                jumpData.jumpStarted = false;
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0);

                return false;
            }

            // Calc Jump Distance
            float jumpDist = jumpData.jumpStartY + jumpData.jumpCurve.Evaluate(jumpData.jumpCurTime) - tf.position.y;

            // Apply Jump Velocity
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpDist / Time.fixedDeltaTime);

            // Next Step
            jumpData.jumpCurTime += Time.fixedDeltaTime;

            if (!jumpData.jumpStarted && rb2D.velocity.y > 0)
                jumpData.jumpStarted = true;
        }

        return justJumped;
    }
    public static void ResetJumpCount(this ref JumpCurveData jumpData)
    {
        jumpData.curCount = 0;
    }

    public static bool Jump(this ref JumpData jumpData, ref bool input_Jump, Rigidbody2D rb2D, Transform tf)
    {
        jumpData.ResetJumpingState(rb2D, tf);

        if (!input_Jump || !jumpData.canJump)
            return false;

        input_Jump = false;
        jumpData.isJumping = true;
        jumpData.curCount++;
        jumpData.apexY = tf.position.y + jumpData.height;

        // Clamp Jump Velocity
        float jumpVel = jumpData.jumpGravity * jumpData.time;
        jumpVel = Mathf.Clamp(jumpVel, 0f, (jumpData.apexY - tf.position.y) / Time.fixedDeltaTime);

        // Apply Jump Velocity
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpVel);

        return true;
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

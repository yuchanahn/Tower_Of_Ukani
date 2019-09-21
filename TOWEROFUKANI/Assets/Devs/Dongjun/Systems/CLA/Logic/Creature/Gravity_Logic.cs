using UnityEngine;

[System.Serializable]
public struct GravityData
{
    public float acceleration;
    public float terminalVelocity;// Zero => No Terminal Velocity

    public GravityData(float acceleration, float terminalVelocity)
    {
        this.acceleration = acceleration;
        this.terminalVelocity = terminalVelocity;
    }
}

public static class Gravity_Logic
{
    public static void ApplyGravity(Rigidbody2D rb2D, GravityData gravityData)
    {
        float gravity;

        if (gravityData.terminalVelocity != 0)
            gravity = Mathf.Max(rb2D.velocity.y - (gravityData.acceleration * Time.fixedDeltaTime), -gravityData.terminalVelocity);
        else
            gravity = rb2D.velocity.y - (gravityData.acceleration * Time.fixedDeltaTime);

        rb2D.velocity = new Vector2(rb2D.velocity.x, gravity);
    }
}
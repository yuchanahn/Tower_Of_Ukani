using UnityEngine;

[System.Serializable]
public struct GravityData
{
    public bool useGravity;
    public float acceleration;
    public float terminalVelocity;// Zero => No Terminal Velocity

    public static GravityData Zero => new GravityData(false);

    public GravityData(bool useGravity = true, float accel = 0, float terVel = 0)
    {
        this.useGravity = useGravity;
        acceleration = accel;
        terminalVelocity = terVel;
    }
}

public static class Gravity_Logic
{
    public static void ApplyGravity(Rigidbody2D rb2D, GravityData gravityData)
    {
        if (!gravityData.useGravity)
            return;

        float gravity;

        if (gravityData.terminalVelocity != 0)
            gravity = Mathf.Max(rb2D.velocity.y - (gravityData.acceleration * Time.fixedDeltaTime), -gravityData.terminalVelocity);
        else
            gravity = rb2D.velocity.y - (gravityData.acceleration * Time.fixedDeltaTime);

        rb2D.velocity = new Vector2(rb2D.velocity.x, gravity);
    }
}
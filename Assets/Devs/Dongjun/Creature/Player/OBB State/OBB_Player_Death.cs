using Dongjun.Helper;
using UnityEngine;

public class OBB_Player_Death : OBB_Player_State
{
    public override void OnEnter()
    {
        data.groundDetectionData.Reset_IWSolid_ColSize();
        data.Animator.Play("Death");
    }
    public override void OnFixedUpdate()
    {
        // Gravity
        Gravity_Logic.ApplyGravity(data.RB2D, data.gravityData);

        // Slowdown X Velocity
        data.RB2D.velocity = data.RB2D.velocity.Change(x: data.RB2D.velocity.x * 0.6f);
    }
}

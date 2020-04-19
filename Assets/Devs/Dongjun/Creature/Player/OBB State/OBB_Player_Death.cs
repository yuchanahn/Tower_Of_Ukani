using Dongjun.Helper;
using UnityEngine;
using DG.Tweening;

public class OBB_Player_Death : OBB_Player_State
{
    public override void OnEnter()
    {
        data.Animator.Play("Death");
        data.groundDetectionData.Reset_IWSolid_ColSize();

        Time.timeScale = 0.4f;
        Camera.main.DOOrthoSize(4f, 0.25f).SetEase(Ease.InOutElastic);
    }
    public override void OnFixedUpdate()
    {
        // Gravity
        Gravity_Logic.ApplyGravity(data.RB2D, data.gravityData);

        // Slowdown X Velocity
        data.RB2D.velocity = data.RB2D.velocity.Change(x: data.RB2D.velocity.x * 0.6f);
    }
}

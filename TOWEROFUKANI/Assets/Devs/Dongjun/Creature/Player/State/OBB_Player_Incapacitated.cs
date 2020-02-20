using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_Player_Incapacitated : OBB_Player_State_Base
{
    [SerializeField] private GameObject stunnedRingEffect;

    public override void OnExit()
    {
        stunnedRingEffect.SetActive(false);
    }
    public override void OnUpdate()
    {
        VisualEffect();
        Animation();
    }
    public override void OnFixedUpdate()
    {
        if (!PlayerStatus.IsKnockbacked)
            GM.Player.Data.RB2D.velocity = GM.Player.Data.RB2D.velocity.Change(x: 0);

        // Detect Ground
        data.groundDetectionData.DetectGround(!PlayerStatus.IsKnockbacked, data.RB2D, transform);

        // Gravity
        Gravity_Logic.ApplyGravity(data.RB2D,
            data.groundDetectionData.isGrounded ? GravityData.Zero :
            data.gravityData);
    }

    private void VisualEffect()
    {
        stunnedRingEffect.SetActive(PlayerStatus.IsStunned.Value);
    }
    private void Animation()
    {
        if (PlayerStatus.IsStunned.Value)
        {
            data.Animator.Play("Stunned");
            return;
        }

        if (PlayerStatus.IsKnockbacked)
        {
            // TODO:
            //main.animator.Play("Knockbacked");
            return;
        }
    }
}

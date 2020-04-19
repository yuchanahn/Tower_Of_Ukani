using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_Player_Incapacitated : OBB_Player_State
{
    // Stun
    [SerializeField] private GameObject stunnedRingEffect;

    // Knockback
    private float curKnockbackTime = 0;
    private float curKnockbackGravity = 0;
    private bool playKnockbackAnim = false;

    public override void OnExit()
    {
        // Stun
        stunnedRingEffect.SetActive(false);

        // Knockback
        curKnockbackTime = 0;
        playKnockbackAnim = false;
    }
    public override void OnUpdate()
    {
        VisualEffect();
        Animation();
    }
    public override void OnFixedUpdate()
    {
        // Stunned
        if (!PlayerStatus.IsKnockbacked)
        {
            // 0 X Velocity
            data.RB2D.velocity = data.RB2D.velocity.Change(x: 0);

            // Reset knockback time
            curKnockbackTime = 0;

            // Gravity
            Gravity_Logic.ApplyGravity(data.RB2D,
                data.groundDetectionData.isGrounded ? GravityData.Zero :
                data.gravityData);
        }
        // Knockbacked
        else
        {
            if (data.NewKnockback)
            {
                data.NewKnockback = false;

                // Reset knockback time
                curKnockbackTime = 0;
                
                // Set Current Gravity
                curKnockbackGravity = PlayerStatus.ResetGravityOnKnockback ? 0 : data.RB2D.velocity.y;

                // Start New Knockback Animation
                playKnockbackAnim = true;
            }

            // Gravity
            Gravity_Logic.ApplyGravity(ref curKnockbackGravity,
                data.groundDetectionData.isGrounded ? GravityData.Zero :
                data.gravityData);

            // Calc Knockback Velocity
            Vector2 knockbackVel = PlayerStatus.KnockbackDir.normalized * PlayerStatus.KnockbackCurve.Evaluate(curKnockbackTime);
            curKnockbackTime += Time.fixedDeltaTime;

            // Apply Velocity
            data.RB2D.velocity = knockbackVel + new Vector2(0, curKnockbackGravity);
        }

        // Detect Ground
        data.groundDetectionData.DetectGround(!PlayerStatus.IsKnockbacked || data.RB2D.velocity.y <= 0, data.RB2D, transform);
    }

    private void VisualEffect()
    {
        // Stun Spinning Star Effect
        stunnedRingEffect.SetActive(PlayerStatus.IsStunned.Value);
    }
    private void Animation()
    {
        string GetKnockbackAnim()
        {
            return (PlayerStatus.KnockbackDir.x == data.Dir && PlayerStatus.KnockbackDir.x != 0
                ? "Knockbacked_Back"
                : "Knockbacked_Front");
        }

        if (playKnockbackAnim)
        {
            playKnockbackAnim = false;
            data.Animator.Play(GetKnockbackAnim(), 0, 0);
            return;
        }
        if (PlayerStatus.IsKnockbacked)
        {
            data.Animator.Play(GetKnockbackAnim());
            return;
        }

        if (PlayerStatus.IsStunned.Value)
        {
            data.Animator.Play("Stunned");
            return;
        }
    }
}

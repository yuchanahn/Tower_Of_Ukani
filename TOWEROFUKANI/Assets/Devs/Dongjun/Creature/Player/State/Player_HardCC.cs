using Dongjun.Helper;
using UnityEngine;

public class Player_HardCC : SSM_State_wMain<Player>
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
            GM.Player.RB2D.velocity = GM.Player.RB2D.velocity.Change(x: 0);

        // Detect Ground
        main.groundDetectionData.DetectGround(!PlayerStatus.IsKnockbacked, main.RB2D, transform);

        // Gravity
        Gravity_Logic.ApplyGravity(main.RB2D,
            main.groundDetectionData.isGrounded ? GravityData.Zero :
            main.gravityData);
    }

    private void VisualEffect()
    {
        stunnedRingEffect.SetActive(PlayerStatus.IsStunned.Value);
    }
    private void Animation()
    {
        if (PlayerStatus.IsStunned.Value)
        {
            main.Animator.Play("Stunned");
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

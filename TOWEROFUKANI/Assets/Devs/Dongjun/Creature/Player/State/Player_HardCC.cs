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
        if (!PlayerStatus.Inst.IsKnockbacked)
            GM.Player.rb2D.velocity = GM.Player.rb2D.velocity.Change(x: 0);

        // Detect Ground
        main.groundDetectionData.DetectGround(!PlayerStatus.Inst.IsKnockbacked, main.rb2D, transform);

        // Gravity
        Gravity_Logic.ApplyGravity(main.rb2D,
            main.groundDetectionData.isGrounded ? GravityData.Zero :
            main.gravityData);
    }

    private void VisualEffect()
    {
        stunnedRingEffect.SetActive(PlayerStatus.Inst.IsStunned.Value);
    }
    private void Animation()
    {
        if (PlayerStatus.Inst.IsStunned.Value)
        {
            main.animator.Play("Stunned");
            return;
        }

        if (PlayerStatus.Inst.IsKnockbacked)
        {
            // TODO:
            //main.animator.Play("Knockbacked");
            return;
        }
    }
}

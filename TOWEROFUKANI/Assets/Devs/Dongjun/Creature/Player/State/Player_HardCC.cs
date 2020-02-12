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
        main.groundDetectionData.DetectGround(true /*넉백 될 때는 안해야 함.*/, main.rb2D, transform);

        // TODO: 넉백 도중에는 콜라이더 크기 리셋
        // main.groundDetectionData.Reset_IW_Solid_Col_Size();

        Gravity_Logic.ApplyGravity(main.rb2D,
            main.groundDetectionData.isGrounded ? GravityData.Zero :
            main.gravityData);
    }

    private void VisualEffect()
    {
        stunnedRingEffect.SetActive(PlayerStatus.Inst.IsStunned);
    }
    private void Animation()
    {
        if (PlayerStatus.Inst.IsStunned)
        {
            main.animator.Play("Stunned");
            return;
        }
    }
}

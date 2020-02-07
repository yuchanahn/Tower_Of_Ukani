using UnityEngine;

public class Player_Stunned : SSM_State_wMain<Player>
{
    [SerializeField] private GameObject stunnedRingEffect;

    public override void OnEnter()
    {
        stunnedRingEffect.SetActive(true);
        main.animator.Play("Stunned");
    }
    public override void OnExit()
    {
        stunnedRingEffect.SetActive(false);
    }
    public override void OnFixedUpdate()
    {
        Gravity_Logic.ApplyGravity(main.rb2D,
            main.groundDetectionData.isGrounded ? GravityData.Zero : main.gravityData);
    }
}

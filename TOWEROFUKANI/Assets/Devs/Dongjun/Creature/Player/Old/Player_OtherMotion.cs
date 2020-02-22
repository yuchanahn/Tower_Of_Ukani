using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_OtherMotion : SSM_State_wMain<Player>
{
    public override void OnFixedUpdate()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        // TODO : 다른 모션 플레이 하든지 말든지

        const string
            Idle = "Idle",
            Airborne = "Airborne";

        main.Animator.Play(main.groundDetectionData.isGrounded ? Idle : Airborne);
    }
}

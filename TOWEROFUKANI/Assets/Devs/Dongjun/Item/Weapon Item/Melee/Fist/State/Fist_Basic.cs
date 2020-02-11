using Dongjun.Helper;
using UnityEngine;

public class Fist_Basic : Melee_State_Base<FistItem>
{
    public override void OnEnter()
    {
        // Animation
        weapon.animator.ResetSpeed();
        weapon.animator.Play(weapon.ANIM_Neutral);
    }
    public override void OnLateUpdate()
    {
        if (!weapon.IsSelected)
            return;

        // Look At Mouse
        transform.LookAtMouseFlipX(Global.Inst.MainCam, transform);
    }
}

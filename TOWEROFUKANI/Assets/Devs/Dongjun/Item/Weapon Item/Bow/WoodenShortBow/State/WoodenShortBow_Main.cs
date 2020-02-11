﻿using Dongjun.Helper;
using UnityEngine;

public class WoodenShortBow_Main : Bow_State_Base<WoodenShortBowItem>
{
    #region Method: SSM
    public override void OnEnter()
    {
        // Animation
        weapon.animator.ResetSpeed();
        weapon.animator.Play(weapon.ANIM_Idle);
    }
    public override void OnLateUpdate()
    {
        if (PlayerStatus.Inst.IsStunned || !weapon.IsSelected)
            return;

        // Look At Mouse
        transform.AimMouse(Global.Inst.MainCam, transform);
    }
    #endregion
}

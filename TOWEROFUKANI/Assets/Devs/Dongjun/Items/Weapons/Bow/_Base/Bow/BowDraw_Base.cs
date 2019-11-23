﻿using UnityEngine;

public class BowDraw_Base<TItem> : BowAction_Base<TItem>
    where TItem : BowItem
{
    #region Var: Propertis
    public bool IsDrawing 
    { get; protected set; } = false;
    #endregion

    #region Method: CLA_Action
    public override void OnEnter()
    {
        // Reset Data
        weapon.drawPower = 0;
        IsDrawing = true;

        // Start Timer
        weapon.drawTimer.SetActive(true);
        weapon.drawTimer.Restart();

        // Animation
        animator.Play(weapon.ANIM_Draw, 0, 0);
    }
    public override void OnLateEnter()
    {
        // Animation Speed
        animator.SetSpeed(weapon.drawTimer.EndTime.Value, weapon.ANIM_Draw);
    }
    public override void OnExit()
    {
        // Stop Timer
        weapon.drawTimer.SetActive(false);
        weapon.drawTimer.ToZero();

        // Animation
        animator.ResetSpeed();
    }
    public override void OnUpdate()
    {
        if (!weapon.IsSelected)
            return;

        // Shoot
        if (Input.GetKeyUp(PlayerWeaponKeys.MainAbility))
        {
            IsDrawing = false;
            weapon.drawPower = weapon.drawTimer.CurTime / weapon.drawTimer.EndTime.Value;
        }
    }
    public override void OnLateUpdate()
    {
        if (!weapon.IsSelected)
            return;

        // Look At Mouse
        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, weapon.SpriteRoot.transform, transform);
    }
    #endregion
}
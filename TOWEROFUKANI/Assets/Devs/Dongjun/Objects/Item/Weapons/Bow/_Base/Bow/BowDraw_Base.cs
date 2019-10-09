using UnityEngine;

public abstract class BowDraw_Base<TMain> : BowAction_Base<TMain> 
    where TMain : Bow
{
    #region Var: Properties
    public bool IsDrawing { get; private set; } = false;
    #endregion


    #region Method: CLA_Action
    public override void OnEnter()
    {
        // Start Timer
        weapon.drawTimer.SetActive(true);
        weapon.drawTimer.ToZero();
        weapon.drawTimer.Restart();

        IsDrawing = true;
        weapon.drawPower = 0;

        // Animation
        animator.Play(weapon.Info.NameTrimed + "_Pull", 0, 0);
    }
    public override void OnLateEnter()
    {
        // Set Animation Speed
        Anim_Logic.SetAnimSpeed(animator, weapon.drawTimer.EndTime, weapon.Info.NameTrimed + "_Pull");
    }
    public override void OnExit()
    {
        // Stop Timer
        weapon.drawTimer.SetActive(false);

        IsDrawing = false;

        // Animation
        animator.speed = 1;
        animator.Play(weapon.Info.NameTrimed + "_Idle");
    }
    public override void OnUpdate()
    {
        if (Input.GetKeyUp(PlayerWeaponKeys.MainAbility))
        {
            IsDrawing = false;
            weapon.hasBeenDrawn = true;
            weapon.drawPower = weapon.drawTimer.CurTime / weapon.drawTimer.EndTime;
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

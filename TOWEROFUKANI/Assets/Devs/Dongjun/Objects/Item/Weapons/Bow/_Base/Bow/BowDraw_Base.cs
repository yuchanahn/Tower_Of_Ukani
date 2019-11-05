using UnityEngine;

public abstract class BowDraw_Base<TItem> : BowAction_Base<TItem>
    where TItem : BowItem
{
    #region Var: Properties
    public bool IsDrawing { get; private set; } = false;
    #endregion

    #region Method: CLA_Action
    public override void OnEnter()
    {
        // Start Timer
        weapon.drawTimer.SetActive(true);
        weapon.drawTimer.Restart();

        IsDrawing = true;
        weapon.hasBeenDrawn = false;
        weapon.drawPower = 0;

        // Animation
        animator.Play(weapon.Info.NameTrimed + "_Pull", 0, 0);
    }
    public override void OnLateEnter()
    {
        // Set Animation Speed
        Anim_Logic.SetAnimSpeed(animator, weapon.drawTimer.EndTime.Value, weapon.Info.NameTrimed + "_Pull");
    }
    public override void OnExit()
    {
        // Stop Timer
        weapon.drawTimer.SetActive(false);
        weapon.drawTimer.ToZero();

        IsDrawing = false;

        // Animation
        animator.speed = 1;
        animator.Play(weapon.Info.NameTrimed + "_Idle");
    }
    public override void OnUpdate()
    {
        if (!weapon.IsSelected)
            return;

        if (Input.GetKeyUp(PlayerWeaponKeys.MainAbility))
        {
            IsDrawing = false;
            weapon.hasBeenDrawn = true;
            weapon.drawPower = weapon.drawTimer.CurTime / weapon.drawTimer.EndTime.Value;
        }
    }
    public override void OnLateUpdate()
    {
        if (!weapon.IsSelected)
            return;

        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, weapon.SpriteRoot.transform, transform);
    }
    #endregion
}

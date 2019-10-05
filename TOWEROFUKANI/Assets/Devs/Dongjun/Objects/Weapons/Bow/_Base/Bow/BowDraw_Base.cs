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
        bow.drawTimer.SetActive(true);
        bow.drawTimer.ToZero();
        bow.drawTimer.Restart();

        IsDrawing = true;
        bow.drawPower = 0;

        // Animation
        animator.Play(bow.WeaponNameTrimed + "_Pull", 0, 0);
    }
    public override void OnLateEnter()
    {
        // Set Animation Speed
        Anim_Logic.SetAnimSpeed(animator, bow.drawTimer.EndTime, bow.WeaponNameTrimed + "_Pull");
    }
    public override void OnExit()
    {
        // Stop Timer
        bow.drawTimer.SetActive(false);

        IsDrawing = false;

        // Animation
        animator.speed = 1;
        animator.Play(bow.WeaponNameTrimed + "_Idle");
    }
    public override void OnUpdate()
    {
        if (Input.GetKeyUp(PlayerInputManager.Inst.Keys.MainAbility))
        {
            IsDrawing = false;
            bow.canShoot = true;
            bow.drawPower = bow.drawTimer.CurTime / bow.drawTimer.EndTime;
        }
    }
    public override void OnLateUpdate()
    {
        if (!bow.IsSelected)
            return;

        // Look At Mouse
        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, bow.SpriteRoot.transform, transform);
    }
    #endregion
}

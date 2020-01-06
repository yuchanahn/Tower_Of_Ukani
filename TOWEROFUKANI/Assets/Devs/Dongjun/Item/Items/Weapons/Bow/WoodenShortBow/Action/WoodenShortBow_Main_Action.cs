using UnityEngine;

public class WoodenShortBow_Main_Action : BowAction_Base<WoodenShortBowItem>
{
    #region Method: CLA_Action
    public override void OnEnter()
    {
        // Animation
        animator.ResetSpeed();
        animator.Play(weapon.ANIM_Idle);
    }
    public override void OnLateUpdate()
    {
        if (!weapon.IsSelected)
            return;

        // Look At Mouse
        transform.AimMouse(Global.Inst.MainCam, transform);
    }
    #endregion
}

using Dongjun.Helper;
using UnityEngine;

public class OBB_Bow_Draw_Base<D, W> : AimedWeapon_State_Base<D, W>
    where D : OBB_Data_Animator
    where W : BowItem
{
    public override void OnEnter()
    {
        // Start Timer
        weaponItem.Timer_Draw.SetActive(true);

        // Animation
        data.Animator.Play(weaponItem.ANIM_Draw);
    }
    public override void OnLateEnter()
    {
        // Animation
        data.Animator.SetDuration(weaponItem.Timer_Draw.EndTime.Value);
    }
    public override void OnExit()
    {
        // Stop Timer
        weaponItem.Timer_Draw.SetActive(false);
        weaponItem.Timer_Draw.Reset();
        weaponItem.Timer_Shoot.Reset();

        // Animation
        data.Animator.ResetSpeed();
    }
}

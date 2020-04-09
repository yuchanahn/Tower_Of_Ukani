using Dongjun.Helper;
using UnityEngine;

public class OBB_Bow_Draw_Base<D, W> : AimedWeapon_State_Base<D, W>
    where D : OBB_Data_Animator
    where W : BowItem
{
    public override void OnEnter()
    {
        // Start Timer
        weaponItem.Dur_Main_Draw.SetActive(true);

        // Animation
        data.Animator.Play(weaponItem.ANIM_Draw);
    }
    public override void OnLateEnter()
    {
        // Animation
        data.Animator.SetDuration(weaponItem.Dur_Main_Draw.EndTime.Value);
    }
    public override void OnExit()
    {
        // Stop Timer
        weaponItem.Dur_Main_Draw.SetActive(false);
        weaponItem.Dur_Main_Draw.Reset();

        // Animation
        data.Animator.ResetSpeed();
    }
}

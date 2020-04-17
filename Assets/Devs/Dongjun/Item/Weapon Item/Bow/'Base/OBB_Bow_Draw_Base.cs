using Dongjun.Helper;
using UnityEngine;

public class OBB_Bow_Draw_Base<D, W> : AimedWeapon_State_Base<D, W>
    where D : OBB_Data_Animator
    where W : BowItem
{
    public override void OnEnter()
    {
        // Start Timer
        weaponItem.Main_Draw_Dur.SetActive(true);

        // Animation
        data.Animator.Play(weaponItem.ANIM_Draw);
    }
    public override void OnLateEnter()
    {
        // Animation
        data.Animator.SetDuration(weaponItem.Main_Draw_Dur.EndTime.Value);
    }
    public override void OnExit()
    {
        // Stop Timer
        weaponItem.Main_Draw_Dur.SetActive(false);
        weaponItem.Main_Draw_Dur.Reset();

        // Animation
        data.Animator.ResetSpeed();
    }
}

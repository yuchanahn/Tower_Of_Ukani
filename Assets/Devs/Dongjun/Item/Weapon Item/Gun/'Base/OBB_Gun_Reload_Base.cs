using Dongjun.Helper;
using UnityEngine;

public class OBB_Gun_Reload_Base<D, W> : AimedWeapon_State_Base<D, W>
    where D : OBB_Data_Animator
    where W : GunItem
{
    public override void OnEnter()
    {
        // Timer
        weaponItem.Main_Reload_Dur.SetActive(true);

        // Animation
        data.Animator.Play(weaponItem.ANIM_Reload);
    }
    public override void OnLateEnter()
    {
        // Animation
        data.Animator.SetDuration(weaponItem.Main_Reload_Dur.EndTime.Value);
    }
    public override void OnExit()
    {
        if (weaponItem.Main_Reload_Dur.IsEnded)
            weaponItem.ReloadFull();

        // Timer
        weaponItem.Main_Reload_Dur.SetActive(false);
        weaponItem.Main_Reload_Dur.Reset();

        // Animation
        data.Animator.ResetSpeed();
    }
}

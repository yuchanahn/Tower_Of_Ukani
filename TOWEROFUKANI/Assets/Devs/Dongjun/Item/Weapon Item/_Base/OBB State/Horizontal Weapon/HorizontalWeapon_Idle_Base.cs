using UnityEngine;

public class HorizontalWeapon_Idle_Base<D, W> : Weapon_State_Base<D, W>
    where D : OBB_Data_Animator
    where W : WeaponItem
{
    public override void OnLateUpdate()
    {
        if (PlayerStatus.Incapacitated)
            return;

        // Look At Mouse
        transform.LookAtMouseFlipX(Global.Inst.MainCam, transform);
    }
}

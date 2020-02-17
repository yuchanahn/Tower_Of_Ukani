using UnityEngine;

public class AimedWeapon_State_Base<D, W> : Weapon_State_Base<D, W>
    where D : OBB_Data_Animator
    where W : WeaponItem
{
    public override void OnLateUpdate()
    {
        // Look At Mouse
        transform.AimMouse(Global.Inst.MainCam, transform);
    }
}

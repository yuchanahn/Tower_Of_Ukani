﻿using UnityEngine;

public class AimedWeapon_Idle_Base<D, W> : Weapon_State_Base<D, W>
    where D : OBB_Data_Animator
    where W : WeaponItem
{
    public override void OnLateUpdate()
    {
        if (PlayerStatus.Incapacitated)
            return;

        // Look At Mouse
        transform.AimMouse(CamManager.Inst.MainCam, transform);
    }
}

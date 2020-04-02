﻿using Dongjun.Helper;
using UnityEngine;

public class OBB_Fist_Heavy_Charge : HorizontalWeapon_State_Base<OBB_Data_Fist, FistItem>
{
    private PlayerStatus_Slow status_Slow;

    private void Start()
    {
        status_Slow = new PlayerStatus_Slow(GM.Player.Data.StatusID, GM.Player.gameObject, 50f);
    }

    public override void OnEnter()
    {
        // Timer
        weaponItem.HeavyChargeTime = 0;

        // Status Effect
        PlayerStatus.AddEffect(status_Slow);

        // Animation
        data.Animator.Play("Heavy_Charge");

        // Player
        GM.Player.Data.CanDash = false;
        GM.Player.Data.CanKick = false;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, true);
    }
    public override void OnExit()
    {
        // Status Effect
        PlayerStatus.RemoveEffect(status_Slow);

        // Animation
        data.Animator.ResetSpeed();

        // Player
        GM.Player.Data.CanDash = true;
        GM.Player.Data.CanKick = true;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, false);
    }
    public override void OnUpdate()
    {
        if (weaponItem.HeavyChargeTime < weaponItem.HeavyFullChargeTime)
            weaponItem.HeavyChargeTime += Time.deltaTime;
        else
            weaponItem.HeavyChargeTime = weaponItem.HeavyFullChargeTime;

        // Animation
        data.Animator.speed = Mathf.Lerp(1, 4, weaponItem.HeavyChargeTime / weaponItem.HeavyFullChargeTime);
    }
}

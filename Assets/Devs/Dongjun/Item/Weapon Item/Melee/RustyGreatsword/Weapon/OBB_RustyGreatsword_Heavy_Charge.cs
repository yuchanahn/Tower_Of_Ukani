using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_RustyGreatsword_Heavy_Charge : HorizontalWeapon_State_Base<OBB_Data_RustyGreatsword, RustyGreatswordItem>
{
    private PlayerStatus_Slow status_Slow;

    private void Start()
    {
        status_Slow = new PlayerStatus_Slow(GM.Player.Data.StatusID, GM.Player.gameObject, 60f);
    }

    public override void OnEnter()
    {
        // Timer
        weaponItem.Heavy_CurChargeTime = 0;

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
        if (weaponItem.Heavy_CurChargeTime < weaponItem.Heavy_FullChargeTime)
            weaponItem.Heavy_CurChargeTime += Time.deltaTime;
        else
            weaponItem.Heavy_CurChargeTime = weaponItem.Heavy_FullChargeTime;
    }
}

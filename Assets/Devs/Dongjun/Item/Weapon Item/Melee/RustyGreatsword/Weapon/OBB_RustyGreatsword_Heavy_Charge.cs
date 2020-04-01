using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_RustyGreatsword_Heavy_Charge : Weapon_State_Base<OBB_Data_RustyGreatsword, RustyGreatswordItem>
{
    private PlayerStatus_Slow status_Slow;

    private void Start()
    {
        status_Slow = new PlayerStatus_Slow(GM.Player.Data.StatusID, GM.Player.gameObject, 50f);
    }

    public override void OnEnter()
    {
        weaponItem.HeavyChargeTime = 0;

        // Timer
        weaponItem.Dur_Heavy.SetActive(true);

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
        // Timer
        weaponItem.Dur_Heavy.SetActive(false);
        weaponItem.Dur_Heavy.Reset();

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

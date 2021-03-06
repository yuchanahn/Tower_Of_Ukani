﻿using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_IceStaff_Main : HorizontalWeapon_State_Base<OBB_IceStaff_Data, IceStaffItem>
{
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private IceBolt iceBoltPrefab;

    [Header("Shoot Animation")]
    [SerializeField] private float shootAnimMaxDur;

    public bool IsAnimEnded { get; private set; } = false;

    public override void OnEnter()
    {
        // Timer
        weaponItem.Main_CD.Reset();

        // Animation
        data.Animator.Play("Main_Attack");

        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.WeaponMain);
    }
    public override void OnLateEnter()
    {
        // Animation
        data.Animator.SetDuration(weaponItem.Main_CD.EndTime.Value, shootAnimMaxDur);
    }
    public override void OnExit()
    {
        // Animation
        IsAnimEnded = false;
        data.Animator.ResetSpeed();
    }

    private void SpawnBullet()
    {
        // Check Wall
        if (!ShootCheckWall_Logic.CanShoot(transform, shootPoint))
            return;

        // Look At Mouse
        shootPoint.LookAtMouse(CamManager.Inst.MainCam, shootPoint);

        // Spawn Bullet
        IceBolt iceBolt = iceBoltPrefab.Spawn(shootPoint.position, shootPoint.rotation);

        // Set Bullet Data
        AttackData attackData = weaponItem.AttackData;
        attackData.damageDealer = iceBolt.gameObject;
        iceBolt.InitData(iceBolt.transform.right, weaponItem.Main_IceBoltData, attackData);
    }

    public void OnAnim_Main_Attack()
    {
        SpawnBullet();
    }
    public void OnAnimEnd_Main_Attack()
    {
        IsAnimEnded = true;
    }
}

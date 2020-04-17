using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_IceStaff_Main : Weapon_State_Base<OBB_IceStaff_Data, IceStaffItem>
{
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Bullet bulletPrefab;

    [Header("Shoot Animation")]
    [SerializeField] private float shootAnimMaxDur;

    public bool IsAnimEnded { get; private set; } = false;

    public override void OnEnter()
    {
        // Timer
        weaponItem.Main_Cooldown.Reset();

        // Animation
        data.Animator.Play("Main_Attack");

        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.WeaponMain);
    }
    public override void OnLateEnter()
    {
        // Animation
        data.Animator.SetDuration(weaponItem.Main_Cooldown.EndTime.Value, shootAnimMaxDur);
    }
    public override void OnExit()
    {
        // Animation
        IsAnimEnded = false;
        data.Animator.ResetSpeed();
    }
    public override void OnLateUpdate()
    {
        // Look At Mouse
        transform.LookAtMouseFlipX(CamManager.Inst.MainCam, transform);
        shootPoint.LookAtMouse(CamManager.Inst.MainCam, shootPoint);
    }

    private void SpawnBullet()
    {
        // Check Wall
        if (!ShootCheckWall_Logic.CanShoot(transform, shootPoint))
            return;

        // Spawn Bullet
        Bullet bullet = bulletPrefab.Spawn(shootPoint.position, shootPoint.rotation);

        // Set Bullet Data
        bullet.InitData(bullet.transform.right, weaponItem.Main_BulletData, weaponItem.AttackData);
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

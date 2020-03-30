using Dongjun.Helper;
using System;
using System.Collections.Generic;
using UnityEngine;

class OBB_Shotgun_Main : AimedWeapon_State_Base<OBB_Data_Shotgun, ShotgunItem>
{
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Bullet bulletPrefab;

    [Header("Shoot Animation")]
    [SerializeField] private float shootAnimMaxDur;

    [Header("Muzzle Flash")]
    [SerializeField] private PoolingObj muzzleFlashPrefab;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;

    private ShootCheckWallData shootCheckWallData = new ShootCheckWallData(1.3125f);

    public override void OnEnter()
    {
        // Shoot
        SpawnBullet();
        VisualEffect();

        // Timer
        weaponItem.Timer_Shoot.Reset();

        // Animation
        data.Animator.Play(weaponItem.ANIM_Shoot);

        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.WeaponMain);
        PlayerActionEventManager.Trigger(PlayerActions.GunShoot);
    }
    public override void OnLateEnter()
    {
        // Animation
        data.Animator.SetDuration(weaponItem.Timer_Shoot.EndTime.Value, shootAnimMaxDur);
    }
    public override void OnExit()
    {
        // Animation
        data.Animator.ResetSpeed();
    }

    private void SpawnBullet()
    {
        // Consume Bullet
        weaponItem.UseBullet(1);

        // Check Wall
        if (!shootCheckWallData.CanShoot(transform))
            return;

        // Set Attack Data
        AttackData curAttackData = weaponItem.AttackData;
        curAttackData.damage = new FloatStat(weaponItem.AttackData.damage.Value / weaponItem.PelletCount);

        // Set Bullet Rotation
        Vector3 rot = transform.eulerAngles;
        rot.z -= ((weaponItem.PelletCount / 2) - (weaponItem.PelletCount % 2 == 0 ? 0.5f : 0)) * weaponItem.PelletAngle;
        for (int i = 0; i < weaponItem.PelletCount; i++, rot.z += weaponItem.PelletAngle)
        {
            // Spawn Bullets
            Bullet bullet = bulletPrefab.Spawn(shootPoint.position, Quaternion.Euler(rot));

            // Set Bullet Data
            bullet.InitData(bullet.transform.right, weaponItem.BulletData, curAttackData);
        }
    }
    private void VisualEffect()
    {
        // Muzzle Flash
        muzzleFlashPrefab.Spawn(shootPoint, new Vector2(0, 0), Quaternion.identity);

        // Cam Shake
        CamShake_Logic.ShakeDir(camShakeData_Shoot, transform, Vector2.left);
    }
}


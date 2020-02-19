using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_MachineGun_Main : AimedWeapon_State_Base<OBB_Data_MachineGun, MachineGunItem>
{
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Bullet bulletPrefab;

    [Header("Shoot Animation")]
    [SerializeField] private float shootAnimMaxDur;

    [Header("Accuracy")]
    [SerializeField] private float acry_YPosOffset;
    [SerializeField] private float acry_ZRotOffset;

    [Header("Muzzle Flash")]
    [SerializeField] private PoolingObj muzzleFlashPrefab;

    [Header("Empty Shell")]
    [SerializeField] private Transform emptyShellSpawnPos;
    [SerializeField] private MachineGun_EmptyShell emptyShellPrefab;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;

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
        ActionEffectManager.Trigger(PlayerActions.WeaponMain);
        ActionEffectManager.Trigger(PlayerActions.GunShoot);
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
        // Spawn Bullet
        Bullet bullet = bulletPrefab.Spawn(
            shootPoint.position + (shootPoint.up * Random.Range(-acry_YPosOffset, acry_YPosOffset)),
            Quaternion.Euler(transform.eulerAngles.Add(z: Random.Range(-acry_ZRotOffset, acry_ZRotOffset))));

        // Set Bullet Data
        bullet.InitData(bullet.transform.right, weaponItem.BulletData, weaponItem.AttackData);

        // Consume Bullet
        weaponItem.UseBullet(1);
    }
    private void VisualEffect()
    {
        // Muzzle Flash
        muzzleFlashPrefab.Spawn(shootPoint, new Vector2(0, 0), Quaternion.identity);

        // Empty Shell
        emptyShellPrefab.Spawn(emptyShellSpawnPos.position, transform.rotation);

        // Update Ammo Belt Pos
        weaponItem.UpdateAmmoBeltPos();

        // Cam Shake
        CamShake_Logic.ShakeDir(camShakeData_Shoot, transform, Vector2.left);
    }
}

﻿    using UnityEngine;

public class Pistol_Main_Action : GunAction_Base<PistolItem>
{
    #region Var: Inspector
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Bullet bulletPrefab;

    [Header("Shoot Animation")]
    [SerializeField] private float maxShootAnimTime;

    [Header("Muzzle Flash")]
    [SerializeField] private PoolingObj muzzleFlashPrefab;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Stats
    private WeaponProjectileData bulletData;
    #endregion

    #region Var: Properties
    public bool IsAnimEnded_Shoot { get; private set; } = false;
    #endregion

    #region Method: CLA_Action
    public override void OnExit()
    {
        OnAnimEnd_Shoot();
    }
    public override void OnUpdate()
    {
        if (!weapon.IsSelected || weapon.loadedBullets <= 0)
            return;

        // Shoot
        if (weapon.shootTimer.IsEnded && Input.GetKeyDown(PlayerWeaponKeys.MainAbility))
        {
            weapon.shootTimer.Restart();

            Shoot();
            ShootEffects();
            AnimPlay_Shoot();
        }
    }
    public override void OnLateUpdate()
    {
        if (!weapon.IsSelected)
            return;

        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, weapon.SpriteRoot.transform, transform);
        Anim_Logic.SetAnimSpeed(animator, weapon.shootTimer.EndTime.Value, maxShootAnimTime, string.Concat(weapon.Info.NameTrimed, "_Shoot"));
    }
    #endregion

    #region Method: Shoot
    private void Shoot()
    {
        // Set Bullet Data
        bulletData = weapon.bulletData;

        // Spawn Bullet
        Bullet bullet = bulletPrefab.Spawn(shootPoint.position, transform.rotation);
        bullet.SetData(bulletData);

        // Consume Bullet
        weapon.loadedBullets -= 1;

        // Trigger Item Effect
        ItemEffectManager.Trigger(PlayerActions.WeaponMain);
        ItemEffectManager.Trigger(PlayerActions.GunShoot);
    }
    private void ShootEffects()
    {
        // Muzzle Flash
        muzzleFlashPrefab.Spawn(shootPoint, new Vector2(0, 0), Quaternion.identity);

        // Cam Shake
        CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
    }
    private void AnimPlay_Shoot()
    {
        IsAnimEnded_Shoot = false;
        animator.Play(string.Concat(weapon.Info.NameTrimed, "_Shoot"), 0, 0);
    }
    #endregion

    #region Method: Anim Event
    private void OnAnimEnd_Shoot()
    {
        IsAnimEnded_Shoot = true;
        animator.speed = 1;
    }
    #endregion
}

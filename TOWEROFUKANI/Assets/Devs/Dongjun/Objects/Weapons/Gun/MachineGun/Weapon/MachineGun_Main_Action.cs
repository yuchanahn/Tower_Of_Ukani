﻿using UnityEngine;

public class MachineGun_Main_Action : GunAction_Base<MachineGun>
{
    #region Var: Inspector
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private WeaponProjectileData bulletData;

    [Header("Shoot Animation")]
    [SerializeField] private float maxShootAnimTime;

    [Header("Accuracy")]
    [SerializeField] private float acry_YPosOffset;
    [SerializeField] private float acry_ZRotOffset;

    [Header("Muzzle Flash")]
    [SerializeField] private PoolingObj muzzleFlashPrefab;

    [Header("Empty Shell")]
    [SerializeField] private Transform emptyShellSpawnPos;
    [SerializeField] private PoolingObj emptyShellPrefab;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Animation
    const string ANIM_T_Shoot = "Shoot";
    const string ANIM_S_Shoot = "MachineGun_Shoot";
    #endregion

    #region Var: Properties
    public bool IsAnimEnded_Shoot { get; private set; } = false;
    #endregion


    #region Method: CLA_Action
    public override void OnLateEnter()
    {
        UpdateAmmoBeltPos();
    }
    public override void OnExit()
    {
        AnimReset_Shoot();
    }
    public override void OnUpdate()
    {
        if (!gun.IsSelected || gun.loadedBullets <= 0)
            return;

        // Shoot
        if (gun.shootTimer.IsEnded && Input.GetKey(PlayerInputManager.Inst.Keys.MainAbility))
        {
            gun.shootTimer.Restart();

            Shoot();
            ShootEffects();
            AnimPlay_Shoot();
        }
    }
    public override void OnLateUpdate()
    {
        if (!gun.IsSelected)
            return;

        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, gun.SpriteRoot.transform, transform);
        AnimSetSpeed_Shoot();
    }
    #endregion

    #region Method: Shoot
    private void Shoot()
    {
        // Spawn Bullet
        Bullet bullet = bulletPrefab.Spawn(shootPoint.position, transform.rotation);
        bullet.transform.position += shootPoint.up * Random.Range(-acry_YPosOffset, acry_YPosOffset);
        bullet.transform.rotation = Quaternion.Euler(0, 0, bullet.transform.eulerAngles.z + Random.Range(-acry_ZRotOffset, acry_ZRotOffset));
        bullet.SetData(bulletData);

        // Consume Bullet
        gun.loadedBullets -= 1;
    }
    private void ShootEffects()
    {
        // Update Ammo Belt Pos
        UpdateAmmoBeltPos();

        // Empty Shell
        emptyShellPrefab.Spawn(emptyShellSpawnPos.position, transform.rotation);

        // Muzzle Flash
        muzzleFlashPrefab.Spawn(shootPoint, new Vector2(0, 0), Quaternion.identity);

        // Cam Shake
        CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
    }
    private void UpdateAmmoBeltPos()
    {
        gun.ammoBelt.localPosition =
            new Vector2(0, Mathf.Lerp(0, gun.AmmoBeltMaxY, 1 - ((float)gun.loadedBullets / gun.magazineSize)));
    }

    // Animation
    private void AnimPlay_Shoot()
    {
        IsAnimEnded_Shoot = false;
        animator.SetTrigger(ANIM_T_Shoot);
    }
    private void AnimSetSpeed_Shoot()
    {
        float maxDuration = maxShootAnimTime > 0 ? maxShootAnimTime : gun.shootTimer.EndTime;
        Anim_Logic.SetAnimSpeed(animator, gun.shootTimer.EndTime, maxDuration, ANIM_S_Shoot);
    }
    private void AnimReset_Shoot()
    {
        animator.speed = 1;
        animator.ResetTrigger(ANIM_T_Shoot);
    }
    #endregion

    #region Method: Anim Event
    private void OnAnimEnd_Shoot()
    {
        IsAnimEnded_Shoot = true;
    }
    #endregion
}
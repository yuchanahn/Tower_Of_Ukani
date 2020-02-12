using Dongjun.Helper;
using UnityEngine;

public class MachineGun_Main : Gun_State_Base<MachineGunItem>
{
    #region Var: Inspector
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Bullet bulletPrefab;

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

    #region Var: Properties
    public bool IsAnimEnded_Shoot { get; private set; } = false;
    #endregion

    #region Method: SSM
    public override void OnLateEnter()
    {
        UpdateAmmoBeltPos();
    }
    public override void OnExit()
    {
        OnAnimEnd_Shoot();
    }
    public override void OnUpdate()
    {
        if (!weapon.IsSelected || weapon.loadedBullets <= 0)
            return;

        if (PlayerStatus.Inst.IsHardCCed)
            return;

        if (IsAnimEnded_Shoot)
            weapon.animator.Play(weapon.ANIM_Idle);

        Shoot();
    }
    public override void OnLateUpdate()
    {
        if (!weapon.IsSelected)
            return;

        if (PlayerStatus.Inst.IsHardCCed || !weapon.IsSelected)
            return;

        // Look At Mouse
        transform.AimMouse(Global.Inst.MainCam, transform);

        // Animation Speed
        weapon.animator.SetDuration(weapon.shootTimer.EndTime.Value, maxShootAnimTime, weapon.ANIM_Shoot);
    }
    #endregion

    #region Method: Shoot
    private void Shoot()
    {
        if (!weapon.shootTimer.IsEnded || !PlayerWeaponKeys.GetKey(PlayerWeaponKeys.MainAbility))
            return;

        weapon.shootTimer.Restart();

        SpawnBullet();
        ShootEffects();

        // Animation
        IsAnimEnded_Shoot = false;
        weapon.animator.Play(weapon.ANIM_Shoot, 0, 0);

        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.WeaponMain);
        ActionEffectManager.Trigger(PlayerActions.GunShoot);
    }
    private void SpawnBullet()
    {
        // Spawn Bullet
        Bullet bullet = bulletPrefab.Spawn(
            shootPoint.position + (shootPoint.up * Random.Range(-acry_YPosOffset, acry_YPosOffset)),
            Quaternion.Euler(transform.eulerAngles.Add(z: Random.Range(-acry_ZRotOffset, acry_ZRotOffset))));

        // Set Bullet Data
        bullet.InitData(bullet.transform.right, weapon.bulletData, weapon.attackData);

        // Consume Bullet
        weapon.loadedBullets -= 1;
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
        weapon.ammoBelt.localPosition =
            new Vector2(0, Mathf.Lerp(0, weapon.AmmoBeltMaxY, 1 - ((float)weapon.loadedBullets / weapon.magazineSize.Value)));
    }
    #endregion

    #region Method: Anim Event
    private void OnAnimEnd_Shoot()
    {
        IsAnimEnded_Shoot = true;
        weapon.animator.ResetSpeed();
    }
    #endregion
}

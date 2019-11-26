    using UnityEngine;

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

        if(weapon.shootTimer.IsEnded)
        {
            animator.Play(weapon.ANIM_Idle);

            if (Input.GetKeyDown(PlayerWeaponKeys.MainAbility))
            {
                weapon.shootTimer.Restart();
                Shoot();
            }
        }
    }
    public override void OnLateUpdate()
    {
        if (!weapon.IsSelected)
            return;

        // Look At Mouse
        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, weapon.SpriteRoot.transform, transform);

        // Animation Speed
        animator.SetSpeed(weapon.shootTimer.EndTime.Value, maxShootAnimTime, weapon.ANIM_Shoot);
    }
    #endregion

    #region Method: Shoot
    private void Shoot()
    {
        SpawnBullet();
        ShootEffects();

        // Play Animation
        IsAnimEnded_Shoot = false;
        animator.Play(weapon.ANIM_Shoot, 0, 0);

        // Trigger Item Effect
        ItemEffectManager.Trigger(PlayerActions.WeaponMain);
        ItemEffectManager.Trigger(PlayerActions.GunShoot);
    }
    private void SpawnBullet()
    {
        // Spawn Bullet
        Bullet bullet = bulletPrefab.Spawn(shootPoint.position, transform.rotation);

        // Set Bullet Data
        bullet.InitData(weapon.bulletData, weapon.attackData);

        // Consume Bullet
        weapon.loadedBullets -= 1;
    }
    private void ShootEffects()
    {
        // Muzzle Flash
        muzzleFlashPrefab.Spawn(shootPoint, new Vector2(0, 0), Quaternion.identity);

        // Cam Shake
        CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
    }
    #endregion

    #region Method: Anim Event
    private void OnAnimEnd_Shoot()
    {
        IsAnimEnded_Shoot = true;
        animator.ResetSpeed();
    }
    #endregion
}

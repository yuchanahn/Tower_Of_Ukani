using UnityEngine;

public class Shotgun_Main_Action : GunAction_Base<ShotgunItem>
{
    #region Var: Inspector
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Bullet bulletPrefab;

    [Header("Shoot Animation")]
    [SerializeField] private float maxShootAnimTime;

    [Header("Pellets")]
    [SerializeField] private int pelletCount = 2;
    [SerializeField] private float pelletAngle = 10f;

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
        if (!weapon.IsSelected || !weapon.isBulletLoaded)
            return;

        // Shoot
        if (weapon.shootTimer.IsEnded)
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
        transform.AimMouse(Global.Inst.MainCam, transform);

        // Animation Speed
        animator.SetSpeed(weapon.shootTimer.EndTime.Value, maxShootAnimTime, weapon.ANIM_Shoot);
    }
    #endregion

    #region Method: Shoot
    private void Shoot()
    {
        SpawnBullet();
        ShootEffects();

        // Animtaion
        IsAnimEnded_Shoot = false;
        animator.Play(weapon.ANIM_Shoot, 0, 0);

        // Trigger Item Effect
        ItemEffectManager.Trigger(PlayerActions.WeaponMain);
        ItemEffectManager.Trigger(PlayerActions.GunShoot);
    }
    private void SpawnBullet()
    {
        // Set Attack Data
        AttackData curAttackData = weapon.attackData;
        curAttackData.damage = new IntStat(MathD.Round((float)weapon.attackData.damage.Value / pelletCount));

        // Set Bullet Rotation
        Vector3 rot = transform.eulerAngles;
        rot.z -= ((pelletCount / 2) - (pelletCount % 2 == 0 ? 0.5f : 0)) * pelletAngle;
        for (int i = 0; i < pelletCount; i++, rot.z += pelletAngle)
        {
            // Spawn Bullets
            Bullet bullet = bulletPrefab.Spawn(shootPoint.position, Quaternion.Euler(rot));

            // Set Bullet Data
            bullet.InitData(bullet.transform.right, weapon.bulletData, curAttackData);
        }

        // Consume Bullet
        weapon.loadedBullets -= 1;
        weapon.isBulletLoaded = false;
    }
    private void ShootEffects()
    {
        // Muzzle Flash
        muzzleFlashPrefab.Spawn(shootPoint, new Vector2(0, 0), Quaternion.identity);

        // Cam Shake Effect
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
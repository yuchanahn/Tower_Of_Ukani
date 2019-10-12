using UnityEngine;

public class Shotgun_Main_Action : GunAction_Base<Shotgun>
{
    #region Var: Inspector
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private WeaponProjectileData bulletData;

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


    protected override void Awake()
    {
        base.Awake();
        bulletData.Init();
    }

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
        Anim_Logic.SetAnimSpeed(animator, weapon.shootTimer.EndTime.Cur, maxShootAnimTime, string.Concat(weapon.Info.NameTrimed, "_Shoot"));
    }
    #endregion

    #region Method: Shoot
    private void Shoot()
    {
        // Spawn Bullets
        Vector3 eRot = transform.eulerAngles;
        eRot.z -= ((pelletCount / 2) - (pelletCount % 2 == 0 ? 0.5f : 0)) * pelletAngle;

        for (int i = 0; i < pelletCount; i++)
        {
            Bullet bullet = bulletPrefab.Spawn(shootPoint.position, Quaternion.Euler(eRot));
            bullet.SetData(bulletData);

            eRot.z += pelletAngle;
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

    // Animation
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
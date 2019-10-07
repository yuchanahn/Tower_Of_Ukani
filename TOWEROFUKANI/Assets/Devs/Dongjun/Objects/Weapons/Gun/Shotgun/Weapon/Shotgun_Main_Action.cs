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


    #region Method: CLA_Action
    public override void OnExit()
    {
        OnAnimEnd_Shoot();
    }
    public override void OnUpdate()
    {
        if (!gun.IsSelected || !gun.isBulletLoaded)
            return;

        // Shoot
        if (gun.shootTimer.IsEnded && Input.GetKeyDown(PlayerWeaponKeys.MainAbility))
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
        Anim_Logic.SetAnimSpeed(animator, gun.shootTimer.EndTime, maxShootAnimTime, string.Concat(gun.WeaponNameTrimed, "_Shoot"));
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
        gun.loadedBullets -= 1;
        gun.isBulletLoaded = false;
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
        animator.Play(string.Concat(gun.WeaponNameTrimed, "_Shoot"), 0, 0);
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
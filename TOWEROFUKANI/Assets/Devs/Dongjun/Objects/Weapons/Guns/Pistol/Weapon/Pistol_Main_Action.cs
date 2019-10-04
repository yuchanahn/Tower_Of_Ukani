using UnityEngine;

public class Pistol_Main_Action : GunAction_Base<Pistol>
{
    #region Var: Inspector
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private BulletData bulletData;

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
        AnimReset_Shoot();
    }
    public override void OnUpdate()
    {
        if (!gun.IsSelected || gun.loadedBullets <= 0)
            return;

        // Shoot
        if (gun.shootTimer.IsEnded && Input.GetKeyDown(PlayerInputManager.Inst.Keys.MainAbility))
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
        bullet.SetData(bulletData);

        // Consume Bullet
        gun.loadedBullets -= 1;
    }
    private void ShootEffects()
    {
        // Muzzle Flash
        muzzleFlashPrefab.Spawn(shootPoint, new Vector2(0, 0), Quaternion.identity);

        // Cam Shake
        CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
    }

    // Animation
    private void AnimPlay_Shoot()
    {
        IsAnimEnded_Shoot = false;
        animator.SetTrigger("Shoot");
    }
    private void AnimSetSpeed_Shoot()
    {
        float maxDuration = maxShootAnimTime > 0 ? maxShootAnimTime : gun.shootTimer.EndTime;
        Anim_Logic.SetAnimSpeed(animator, gun.shootTimer.EndTime, maxDuration, string.Concat(gun.WeaponNameTrimed, "_Shoot"));
    }
    private void AnimReset_Shoot()
    {
        animator.speed = 1;
        animator.ResetTrigger("Shoot");
    }
    #endregion

    #region Method: Anim Event
    private void OnAnimEnd_Shoot()
    {
        IsAnimEnded_Shoot = true;
    }
    #endregion
}

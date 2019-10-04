using UnityEngine;

public class WoodenShortBow_Main_Action : BowAction_Base<WoodenShortBow>
{
    #region Var: Inspector
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private BulletData bulletData;

    [Header("Shoot Animation")]
    [SerializeField] private float maxShootAnimTime;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Properties
    public bool IsAnimEnded_Shoot { get; private set; } = true;
    #endregion


    #region Method: CLA_Action
    public override void OnEnter()
    {
        bow.shootTimer.SetActive(true);
        bow.shootTimer.Restart();
    }
    public override void OnLateEnter()
    {
        AnimSetSpeed_Shoot();
    }
    public override void OnExit()
    {
        AnimReset_Shoot();
    }
    public override void OnUpdate()
    {
        if (bow.canShoot)
        {
            bow.canShoot = false;

            IsAnimEnded_Shoot = false;
            AnimPlay_Shoot();
        }
        else if (IsAnimEnded_Shoot)
        {
            AnimPlay_Idle();
        }
    }
    public override void OnLateUpdate()
    {
        if (!bow.IsSelected)
            return;

        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, bow.SpriteRoot.transform, transform);
    }
    #endregion

    #region Method: Idle
    private void AnimPlay_Idle()
    {
        animator.Play(string.Concat(bow.WeaponNameTrimed, "_Idle"));
    }
    #endregion

    #region Method: Shoot
    private void Shoot()
    {
        // Spawn Bullet
        Bullet bullet = bulletPrefab.Spawn(shootPoint.position, transform.rotation);
        bullet.SetData(bulletData);
    }
    private void ShootEffects()
    {
        // Cam Shake
        CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
    }

    // Animation
    private void AnimPlay_Shoot()
    {
        animator.Play(string.Concat(bow.WeaponNameTrimed, "_Shoot"), 0, 0);
    }
    private void AnimSetSpeed_Shoot()
    {
        float maxDuration = maxShootAnimTime > 0 ? maxShootAnimTime : bow.shootTimer.EndTime;
        Anim_Logic.SetAnimSpeed(animator, bow.shootTimer.EndTime, maxDuration, string.Concat(bow.WeaponNameTrimed, "_Shoot"));
    }
    private void AnimReset_Shoot()
    {
        animator.speed = 1;
    }
    #endregion

    #region Method: Anim Event
    private void OnAnim_ShootArrow()
    {
        Shoot();
        ShootEffects();
    }
    private void OnAnimEnd_Shoot()
    {
        IsAnimEnded_Shoot = true;
        bow.shootTimer.SetActive(false);

        AnimReset_Shoot();
    }
    #endregion
}

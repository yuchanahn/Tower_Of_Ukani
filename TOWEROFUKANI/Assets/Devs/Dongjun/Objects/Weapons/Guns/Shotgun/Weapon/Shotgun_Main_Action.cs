using UnityEngine;

public class Shotgun_Main_Action : GunAction_Base<Shotgun>
{
    #region Var: Inspector
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private PoolingObj bulletPrefab;
    [SerializeField] private float maxShootAnimTime;

    [Header("Pellets")]
    [SerializeField] private int pelletCount = 2;
    [SerializeField] private float pelletAngle = 10f;

    [Header("Muzzle Flash")]
    [SerializeField] private PoolingObj muzzleFlashPrefab;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Animation
    const string ANIM_T_Shoot = "Shoot";
    const string ANIM_S_Shoot = "Shotgun_Shoot";
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
        if (!gun.IsSelected || !gun.gunData.isBulletLoaded)
            return;

        // Shoot
        if (gun.gunData.shootTimer.IsEnded && Input.GetKeyDown(PlayerInputManager.Inst.Keys.MainAbility))
        {
            gun.gunData.shootTimer.Restart();

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
        // Spawn Bullets
        Vector3 eRot = transform.eulerAngles;
        eRot.z -= ((pelletCount / 2) - (pelletCount % 2 == 0 ? 0.5f : 0)) * pelletAngle;

        for (int i = 0; i < pelletCount; i++)
        {
            bulletPrefab.Spawn(shootPoint.position, Quaternion.Euler(eRot));
            eRot.z += pelletAngle;
        }

        // Consume Bullet
        gun.gunData.loadedBullets -= 1;
        gun.gunData.isBulletLoaded = false;
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
        animator.SetTrigger(ANIM_T_Shoot);
    }
    private void AnimSetSpeed_Shoot()
    {
        Anim_Logic.SetAnimSpeed(animator, gun.gunData.shootTimer.EndTime, maxShootAnimTime > 0 ? maxShootAnimTime : gun.gunData.shootTimer.EndTime, ANIM_S_Shoot);
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
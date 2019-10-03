using UnityEngine;

public class Pistol_Main_Action : GunAction_Base<Pistol>
{
    #region Var: Inspector
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private PoolingObj bulletPrefab;
    [SerializeField] private float maxShootAnimTime;

    [Header("Muzzle Flash")]
    [SerializeField] private Transform muzzleFlashParent;
    [SerializeField] private PoolingObj muzzleFlashPrefab;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Animation
    const string ANIM_T_Shoot = "Shoot";
    const string ANIM_S_Shoot = "Pistol_Shoot";
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
        if (!gun.IsSelected || gun.gunData.loadedBullets <= 0)
            return;

        // Temp!
        // 공격 속도를 업데이트 할 때만 실행되도록 해야함.
        maxShootAnimTime = maxShootAnimTime <= 0 ? gun.gunData.shootTimer.EndTime : maxShootAnimTime;

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
        // Spawn Bullet
        bulletPrefab.Activate(shootPoint.position, transform.rotation);

        // Consume Bullet
        gun.gunData.loadedBullets -= 1;
    }
    private void ShootEffects()
    {
        // Muzzle Flash
        muzzleFlashPrefab.Activate(muzzleFlashParent, new Vector2(0, 0), Quaternion.identity);

        // Cam Shake
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
        Anim_Logic.SetAnimSpeed(animator, gun.gunData.shootTimer.EndTime, maxShootAnimTime, ANIM_S_Shoot);
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

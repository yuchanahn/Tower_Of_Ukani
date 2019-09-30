using UnityEngine;

public class Pistol_Main_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Points")]
    [SerializeField] private Transform shootPoint;

    [Header("Prefabs")]
    [SerializeField] private PoolingObj bulletPrefab;

    [Header("Animation")]
    [SerializeField] private float maxShootAnimTime;

    [Header("Effects")]
    [SerializeField] private Transform muzzleFlashParent;
    [SerializeField] private PoolingObj muzzleFlashPrefab;
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Components
    private Animator animator;
    private Pistol gun_Main;
    #endregion


    #region Method: Unity
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gun_Main = GetComponent<Pistol>();
    }
    #endregion

    #region Method: CLA_Action
    public override void OnEnd()
    {
        animator.speed = 1;
        animator.ResetTrigger("Shoot");
    }
    public override void OnUpdate()
    {
        if (!gun_Main.IsSelected)
            return;

        if (gun_Main.gunData.shootTimer.IsTimerAtMax && Input.GetKeyDown(PlayerInputManager.Inst.Keys.MainAbility))
        {
            // Spawn Bullet
            ObjPoolingManager.Activate(bulletPrefab, shootPoint.position, transform.rotation);

            // Use Bullet
            gun_Main.gunData.loadedBullets -= 1;

            // Continue Timer
            gun_Main.gunData.shootTimer.Restart();

            // Animation
            animator.SetTrigger("Shoot");

            // Particle Effect
            ObjPoolingManager.Activate(muzzleFlashPrefab, muzzleFlashParent, new Vector2(0, 0), Quaternion.identity);

            // Cam Shake Effect
            CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
        }
    }
    public override void OnLateUpdate()
    {
        AnimSpeed_Logic.SetAnimSpeed(animator, gun_Main.gunData.shootTimer.endTime, maxShootAnimTime, "Pistol_Shoot");
        LookAtMouse_Logic.Rotate(Global.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(Global.Inst.MainCam, gun_Main.SpriteRoot.transform, transform);
    }
    #endregion
}

using UnityEngine;

public class Pistol_Main_Action : CLA_Action
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
            // Restart Timer
            gun_Main.gunData.shootTimer.Restart();

            // Spawn Bullet
            bulletPrefab.Activate(shootPoint.position, transform.rotation);

            // Consume Bullet
            gun_Main.gunData.loadedBullets -= 1;

            // Muzzle Flash
            muzzleFlashPrefab.Activate(muzzleFlashParent, new Vector2(0, 0), Quaternion.identity);

            // Animation
            animator.SetTrigger("Shoot");

            // Cam Shake
            CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
        }
    }
    public override void OnLateUpdate()
    {
        // Lool At Mouse
        LookAtMouse_Logic.Rotate(Global.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(Global.Inst.MainCam, gun_Main.SpriteRoot.transform, transform);

        if (gun_Main.IsSelected)
        {
            AnimSpeed_Logic.SetAnimSpeed(animator, gun_Main.gunData.shootTimer.endTime, maxShootAnimTime, "Pistol_Shoot");
        }
    }
    #endregion
}

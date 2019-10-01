using UnityEngine;

public class Shotgun_Main_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private PoolingObj bulletPrefab;
    [SerializeField] private float maxShootAnimTime;
    [SerializeField] private int pelletCount = 2;
    [SerializeField] private float pelletAngle = 10f;

    [Header("Muzzle Flash")]
    [SerializeField] private Transform muzzleFlashParent;
    [SerializeField] private PoolingObj muzzleFlashPrefab;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Animation
    const string ANIM_T_Shoot = "Shoot";
    const string ANIM_S_Shoot = "Shotgun_Shoot";
    #endregion

    #region Var: Components
    private Animator animator;
    private Shotgun gun;
    #endregion

    #region Var: Properties
    public bool IsAnimEnded_Shoot { get; private set; } = false;
    #endregion


    #region Method: Unity
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gun = GetComponent<Shotgun>();

        maxShootAnimTime = maxShootAnimTime <= 0 ? gun.gunData.shootTimer.EndTime : maxShootAnimTime;
    }
    #endregion

    #region Method: CLA_Action
    public override void OnExit()
    {
        animator.speed = 1;
        animator.ResetTrigger(ANIM_T_Shoot);
    }
    public override void OnUpdate()
    {
        if (!gun.IsSelected || !gun.gunData.isBulletLoaded)
            return;

        if (gun.gunData.shootTimer.IsEnded && Input.GetKeyDown(PlayerInputManager.Inst.Keys.MainAbility))
        {
            // Restart Timer
            gun.gunData.shootTimer.Restart();

            // Spawn Bullets
            Vector3 eRot = transform.eulerAngles;
            eRot.z -= ((pelletCount / 2) - (pelletCount % 2 == 0 ? 0.5f : 0)) * pelletAngle;

            for (int i = 0; i < pelletCount; i++)
            {
                bulletPrefab.Activate(shootPoint.position, Quaternion.Euler(eRot));
                eRot.z += pelletAngle;
            }

            // Consume Bullet
            gun.gunData.loadedBullets -= 1;
            gun.gunData.isBulletLoaded = false;

            // Muzzle Flash
            muzzleFlashPrefab.Activate(muzzleFlashParent, new Vector2(0, 0), Quaternion.identity);

            // Animation
            IsAnimEnded_Shoot = false;
            animator.SetTrigger(ANIM_T_Shoot);

            // Cam Shake Effect
            CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
        }
    }
    public override void OnLateUpdate()
    {
        // Lool At Mouse
        LookAtMouse_Logic.Rotate(Global.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(Global.Inst.MainCam, gun.SpriteRoot.transform, transform);

        // Set Animation Speed
        if (gun.IsSelected)
            Anim_Logic.SetAnimSpeed(animator, gun.gunData.shootTimer.EndTime, maxShootAnimTime, ANIM_S_Shoot);
    }
    #endregion

    #region Method: Anim Event
    private void OnAnimEnd_Shoot()
    {
        IsAnimEnded_Shoot = true;
    }
    #endregion
}
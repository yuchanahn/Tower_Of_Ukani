using UnityEngine;

public class Shotgun_Main_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Stats")]
    [SerializeField] private int pelletCount = 2;
    [SerializeField] private float pelletAngle = 10f;

    [Header("Points")]
    [SerializeField] private Transform shootPoint;

    [Header("Prefabs")]
    [SerializeField] private PoolingObj bulletPrefab;

    [Header("Animation")]
    [SerializeField] private float maxShootAnimTime;

    [Header("Effects")]
    [SerializeField] private Transform shootParticleParent;
    [SerializeField] private PoolingObj shootParticlePrefab;
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Properties
    public bool AnimEnd_Shoot { get; private set; } = false;
    #endregion

    #region Var: Components
    private Animator animator;
    private Shotgun gun_Main;
    #endregion


    #region Method: Unity
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gun_Main = GetComponent<Shotgun>();
    }
    #endregion

    #region Method: CLA_Action
    public override void OnEnd()
    {
        AnimEnd_Shoot = false;
        animator.speed = 1;
        animator.ResetTrigger("Shoot");
    }
    public override void OnUpdate()
    {
        if (!gun_Main.IsSelected)
            return;

        if (gun_Main.gunData.shootTimer.IsTimerAtMax && Input.GetKeyDown(PlayerInputManager.Inst.Keys.MainAbility))
        {
            // Spawn Bullets
            Vector3 eRot = transform.eulerAngles;
            eRot.z -= ((pelletCount / 2) - (pelletCount % 2 == 0 ? 0.5f : 0)) * pelletAngle;
            for (int i = 0; i < pelletCount; i++)
            {
                ObjPoolingManager.Activate(bulletPrefab, shootPoint.position, Quaternion.Euler(eRot));
                eRot.z += pelletAngle;
            }

            // Use a Bullet
            gun_Main.gunData.loadedBullets -= 1;

            // Continue Timer
            gun_Main.gunData.shootTimer.Restart();

            // Animation
            AnimEnd_Shoot = false;
            animator.SetTrigger("Shoot");

            // Particle Effect
            ObjPoolingManager.Activate(shootParticlePrefab, shootParticleParent, new Vector2(0, 0), Quaternion.identity);

            // Cam Shake Effect
            CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
        }
    }
    public override void OnLateUpdate()
    {
        AnimSpeed_Logic.SetAnimSpeed(animator, gun_Main.gunData.shootTimer.endTime, maxShootAnimTime, "Shotgun_Shoot");
        LookAtMouse_Logic.Rotate(Global.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(Global.Inst.MainCam, gun_Main.SpriteRoot.transform, transform);
    }
    #endregion

    #region Method: AnimEvent
    private void OnAnimEnd_Shoot()
    {
        AnimEnd_Shoot = true;
    }
    #endregion
}
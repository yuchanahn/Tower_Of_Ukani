using UnityEngine;

public class Pistol_Main_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Points")]
    [SerializeField] private Transform shootPoint;

    [Header("Prefabs")]
    [SerializeField] private PoolingObj bulletPrefab;

    [Header("Effects")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Components
    private Animator animator;
    private Pistol pistol_Main;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        pistol_Main = GetComponent<Pistol>();
    }
    private void Start()
    {
        pistol_Main.Stats.shootTimer.Init(gameObject);
        pistol_Main.Stats.shootTimer.SetToMax();
        pistol_Main.Stats.loadedBullets = pistol_Main.Stats.magazineSize;
    }

    public override void OnUpdate()
    {
        if (pistol_Main.IsSelected && pistol_Main.Stats.shootTimer.IsTimerAtMax)
        {
            if (Input.GetKeyDown(PlayerInputManager.Inst.Keys.MainAbility))
            {
                // Spawn Bullet
                ObjPoolingManager.Activate(bulletPrefab, shootPoint.position, transform.rotation);

                // Continue Timer
                pistol_Main.Stats.shootTimer.Continue();

                // Animation
                animator.SetTrigger("Shoot");

                // Cam Shake Effect
                CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
            }
        }
    }
    public override void OnLateUpdate()
    {
        LookAtMouse_Logic.Rotate(CommonObjs.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(CommonObjs.Inst.MainCam, pistol_Main.SpriteRoot.transform, transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_Main_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Stats")]
    [SerializeField] private TimerData shootTimer;
    [SerializeField] private int magazineSize = 6;

    [Header("Points")]
    [SerializeField] private Transform shootPoint;

    [Header("Prefabs")]
    [SerializeField] private PoolingObj bulletPrefab;

    [Header("Effects")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Shoot
    private int loadedBulletCount;
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
        shootTimer.Init(gameObject);
        shootTimer.SetToMax();
        loadedBulletCount = magazineSize;
    }

    public override void OnUpdate()
    {
        if (pistol_Main.IsSelected && shootTimer.IsTimerAtMax)
        {
            if (Input.GetKey(PlayerInputManager.Inst.Keys.MainAbility))
            {
                // Spawn Bullet
                ObjPoolingManager.Activate(bulletPrefab, shootPoint.position, transform.rotation);

                // Continue Timer
                shootTimer.Continue();

                // Animation
                animator.SetTrigger("Shoot");

                // Cam Shake Effect
                camShakeData_Shoot.angle = transform.eulerAngles.z - 180f;
                CommonObjs.Inst.CamShake.StartShake(camShakeData_Shoot);
            }
        }
    }
    public override void OnLateUpdate()
    {
        LookAtMouse_Logic.Rotate(CommonObjs.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(CommonObjs.Inst.MainCam, pistol_Main.SpriteRoot.transform, transform);
    }
}

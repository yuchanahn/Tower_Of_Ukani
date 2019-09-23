using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun_Main_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Stats")]
    [SerializeField] private TimerData shootTimer;
    [SerializeField] private int magazineSize = 6;
    [SerializeField] private int pelletCount = 2;
    [SerializeField] private float pelletAngle = 10f;

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
    private Shotgun shotgun_Main;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        shotgun_Main = GetComponent<Shotgun>();
    }
    private void Start()
    {
        shootTimer.Init(gameObject);
        shootTimer.SetToMax();
        loadedBulletCount = magazineSize;
    }

    public override void OnUpdate()
    {
        if (shotgun_Main.IsSelected && shootTimer.IsTimerAtMax)
        {
            if (Input.GetKey(PlayerInputManager.Inst.Keys.MainAbility))
            {
                // Spawn Bullet
                Vector3 eRot = transform.rotation.eulerAngles;
                eRot.z = eRot.z - (((pelletCount / 2) - (pelletCount % 2 == 0 ? 0.5f : 0)) * pelletAngle);
                for (int i = 0; i < pelletCount; i++)
                {
                    ObjPoolingManager.Activate(bulletPrefab, shootPoint.position, Quaternion.Euler(eRot));
                    eRot.z += pelletAngle;
                }

                // Continue Timer
                shootTimer.Continue();

                // Animation
                animator.SetTrigger("Shoot");

                // Cam Shake Effect
                camShakeData_Shoot.angle = transform.eulerAngles.z;
                CommonObjs.Inst.CamShake.StartShake(camShakeData_Shoot);
            }
        }
    }
    public override void OnLateUpdate()
    {
        LookAtMouse_Logic.Rotate(CommonObjs.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(CommonObjs.Inst.MainCam, shotgun_Main.SpriteRoot.transform, transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_Main_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Stats")]
    [SerializeField]
    private TimerData shootTimer;
    [SerializeField]
    private int magazineSize = 6;

    [Header("Points")]
    [SerializeField]
    private Transform shootPoint;

    [Header("Prefabs")]
    [SerializeField]
    private PoolingObj bulletPrefab;
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
    }
    private void Start()
    {
        pistol_Main = GetComponent<Pistol>();

        shootTimer.Init(gameObject);
        shootTimer.SetToMax();

        loadedBulletCount = magazineSize;
    }

    public override void OnUpdate()
    {
        LookAtMouse_Logic.Rotate(CommonObjs.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(CommonObjs.Inst.MainCam, pistol_Main.SpriteRoot.transform, transform);

        if (pistol_Main.IsSelected && shootTimer.IsTimerAtMax)
        {
            if (Input.GetKey(PlayerInputManager.Inst.Keys.MainAbility))
            {
                ObjPoolingManager.Activate(bulletPrefab, shootPoint.position, transform.rotation);
                animator.SetTrigger("Shoot");
                shootTimer.Continue();
            }
        }
    }
}

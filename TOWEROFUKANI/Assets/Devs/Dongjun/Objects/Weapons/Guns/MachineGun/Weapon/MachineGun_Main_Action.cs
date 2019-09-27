using UnityEngine;

public class MachineGun_Main_Action : CLA_Action
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
    private MachineGun machineGun_Main;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        machineGun_Main = GetComponent<MachineGun>();
    }

    public override void OnUpdate()
    {
        if (machineGun_Main.IsSelected && machineGun_Main.Stats.shootTimer.IsTimerAtMax)
        {
            if (Input.GetKey(PlayerInputManager.Inst.Keys.MainAbility))
            {
                // Spawn Bullet
                ObjPoolingManager.Activate(bulletPrefab, shootPoint.position, transform.rotation);

                // Use Bullet
                machineGun_Main.Stats.loadedBullets -= 1;

                // Continue Timer
                machineGun_Main.Stats.shootTimer.Continue();

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
        LookAtMouse_Logic.FlipX(CommonObjs.Inst.MainCam, machineGun_Main.SpriteRoot.transform, transform);
    }
}

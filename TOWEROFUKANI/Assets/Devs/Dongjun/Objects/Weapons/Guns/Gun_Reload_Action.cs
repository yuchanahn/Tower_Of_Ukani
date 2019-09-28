using UnityEngine;

public class Gun_Reload_Action : CLA_Action
{
    #region Var: Inspector
    [SerializeField] private Transform magazineDropPos;
    [SerializeField] private PoolingObj droppedMagazine;
    #endregion

    #region Var: Components
    private Animator animator;
    private Gun gun_Main;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gun_Main = GetComponent<Gun>();
    }

    public override void OnStart()
    {
        // Start Timer
        gun_Main.Stats.reloadTimer.Timer_Cur = 0;
        gun_Main.Stats.reloadTimer.SetActive(true);
        gun_Main.Stats.reloadTimer.Continue();

        // Drop Magazine
        ObjPoolingManager.Activate(droppedMagazine, magazineDropPos.position, transform.rotation);

        // Animation
        animator.ResetTrigger("Shoot");
        animator.Play("Pistol_Reload", 0, 0);
    }
    public override void OnEnd()
    {
        // Stop Timer
        gun_Main.Stats.reloadTimer.SetActive(false);

        // Load Bullets
        if (gun_Main.Stats.reloadTimer.IsTimerAtMax)
            gun_Main.Stats.loadedBullets = gun_Main.Stats.magazineSize;

        // Animation
        animator.speed = 1;
        animator.Play("Pistol_Idle");
    }
    public override void OnLateUpdate()
    {
        AnimSpeed_Logic.SetAnimSpeed(animator, gun_Main.Stats.reloadTimer.Timer_Max, "Pistol_Reload");
        LookAtMouse_Logic.Rotate(CommonObjs.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(CommonObjs.Inst.MainCam, gun_Main.SpriteRoot.transform, transform);
    }
}

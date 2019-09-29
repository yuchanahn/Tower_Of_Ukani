using UnityEngine;

public class Gun_SwapMagazine_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Ammo")]
    [SerializeField] private bool reloadAll = false;
    [SerializeField] private int reloadAmount;

    [Header("Effect")]
    [SerializeField] private Transform magazineDropPos;
    [SerializeField] private PoolingObj droppedMagazinePrefab;
    #endregion

    #region Var: Properties
    public bool AnimStart_SwapMagazine { get; private set; } = false;
    public bool AnimEnd_SwapMagazine { get; private set; } = false;
    #endregion

    #region Var: Components
    private Animator animator;
    private Gun gun;
    #endregion


    #region Method: Unity
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gun = GetComponent<Gun>();
    }
    #endregion

    #region Method: CLA_Action
    public override void OnChange()
    {
        // Start Timer
        gun.Stats.swapMagazineTimer.UseAutoTick(true);
        gun.Stats.swapMagazineTimer.Restart();

        // Animation
        animator.Play(gun.WeaponNameTrimed + "_SwapMagazine", 0, 0);
    }
    public override void OnStart()
    {
        // Set Animation Speed
        AnimSpeed_Logic.SetAnimSpeed(animator, gun.Stats.swapMagazineTimer.Timer_Max, gun.WeaponNameTrimed + "_SwapMagazine");
    }
    public override void OnEnd()
    {
        // Stop Timer
        gun.Stats.swapMagazineTimer.UseAutoTick(false);

        // Animation
        animator.speed = 1;
        animator.Play(gun.WeaponNameTrimed + "_Idle");
    }
    public override void OnLateUpdate()
    {
        // Rotate And Filp
        LookAtMouse_Logic.Rotate(Global.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(Global.Inst.MainCam, gun.SpriteRoot.transform, transform);
    }
    #endregion

    #region Method: AnimEvent
    private void OnAnimStart_SwapMagazine()
    {
        AnimStart_SwapMagazine = true;
        AnimEnd_SwapMagazine = false;
    }
    private void OnAnimEnd_SwapMagazine()
    {
        AnimStart_SwapMagazine = false;
        AnimEnd_SwapMagazine = true;

        // Load Bullets
        gun.Stats.loadedBullets = reloadAll ? gun.Stats.magazineSize : gun.Stats.loadedBullets + reloadAmount;
    }
    private void DropMagazine()
    {
        ObjPoolingManager.Activate(droppedMagazinePrefab, magazineDropPos.position, transform.rotation);
    }
    #endregion
}

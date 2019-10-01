using UnityEngine;

public class Gun_SwapMagazine_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Ammo")]
    [SerializeField] protected bool reloadAll = false;
    [SerializeField] protected int reloadAmount;

    [Header("Effect")]
    [SerializeField] protected Transform magazineDropPos;
    [SerializeField] protected PoolingObj droppedMagazinePrefab;
    #endregion

    #region Var: Properties
    public bool IsAnimStarted_SwapMagazine { get; private set; } = false;
    public bool IsAnimEnded_SwapMagazine { get; private set; } = false;
    #endregion

    #region Var: Components
    protected Animator animator;
    protected Gun gun;
    #endregion


    #region Method: Unity
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        gun = GetComponent<Gun>();
    }
    #endregion

    #region Method: CLA_Action
    public override void OnEnter()
    {
        // Start Timer
        gun.gunData.swapMagazineTimer.Restart();

        IsAnimStarted_SwapMagazine = true;
        IsAnimEnded_SwapMagazine = false;

        // Animation
        animator.Play(gun.WeaponNameTrimed + "_SwapMagazine", 0, 0);
    }
    public override void OnLateEnter()
    {
        // Set Animation Speed
        Anim_Logic.SetAnimSpeed(animator, gun.gunData.swapMagazineTimer.EndTime, gun.WeaponNameTrimed + "_SwapMagazine");
    }
    public override void OnExit()
    {
        // Stop Timer
        gun.gunData.swapMagazineTimer.SetActive(false);

        if (gun.gunData.swapMagazineTimer.IsEnded)
        {
            IsAnimStarted_SwapMagazine = false;
            IsAnimEnded_SwapMagazine = true;

            // Load Bullets
            gun.gunData.loadedBullets = reloadAll ? gun.gunData.magazineSize : gun.gunData.loadedBullets + reloadAmount;
        }

        // Animation
        animator.speed = 1;
        animator.Play(gun.WeaponNameTrimed + "_Idle");
    }
    public override void OnUpdate()
    {
        gun.gunData.swapMagazineTimer.SetActive(gun.IsSelected);
    }
    public override void OnLateUpdate()
    {
        // Rotate And Filp
        LookAtMouse_Logic.Rotate(Global.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(Global.Inst.MainCam, gun.SpriteRoot.transform, transform);
    }
    #endregion

    #region Method: AnimEvent
    protected virtual void DropMagazine()
    {
        ObjPoolingManager.Activate(droppedMagazinePrefab, magazineDropPos.position, transform.rotation);
    }
    #endregion
}

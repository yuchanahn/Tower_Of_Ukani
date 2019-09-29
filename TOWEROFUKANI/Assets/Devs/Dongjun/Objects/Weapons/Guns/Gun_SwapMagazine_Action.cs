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
        gun.gunData.swapMagazineTimer.Restart();

        AnimStart_SwapMagazine = true;
        AnimEnd_SwapMagazine = false;

        // Animation
        animator.Play(gun.WeaponNameTrimed + "_SwapMagazine", 0, 0);
    }
    public override void OnStart()
    {
        // Set Animation Speed
        AnimSpeed_Logic.SetAnimSpeed(animator, gun.gunData.swapMagazineTimer.endTime, gun.WeaponNameTrimed + "_SwapMagazine");
    }
    public override void OnEnd()
    {
        // Stop Timer
        gun.gunData.swapMagazineTimer.SetActive(false);

        if (gun.gunData.swapMagazineTimer.IsTimerAtMax)
        {
            AnimStart_SwapMagazine = false;
            AnimEnd_SwapMagazine = true;

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
    private void DropMagazine()
    {
        ObjPoolingManager.Activate(droppedMagazinePrefab, magazineDropPos.position, transform.rotation);
    }
    #endregion
}

using UnityEngine;

public class Gun_SwapMagazine_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Weapon Data")]
    [SerializeField] private string weaponName;

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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gun = GetComponent<Gun>();
    }

    public override void OnStart()
    {
        // Start Timer
        gun.Stats.swapMagazineTimer.Timer_Cur = 0;
        gun.Stats.swapMagazineTimer.SetActive(true);
        gun.Stats.swapMagazineTimer.Continue();

        // Animation
        animator.Play(weaponName + "_SwapMagazine", 0, 0);
    }
    public override void OnEnd()
    {
        // Stop Timer
        gun.Stats.swapMagazineTimer.SetActive(false);

        // Load Bullets
        if (gun.Stats.swapMagazineTimer.IsTimerAtMax)
            gun.Stats.loadedBullets = reloadAll ? gun.Stats.magazineSize : gun.Stats.loadedBullets + reloadAmount;

        // Animation
        animator.speed = 1;
        animator.Play(weaponName + "_Idle");
    }
    public override void OnLateUpdate()
    {
        AnimSpeed_Logic.SetAnimSpeed(animator, gun.Stats.swapMagazineTimer.Timer_Max, weaponName + "_SwapMagazine");
        LookAtMouse_Logic.Rotate(CommonObjs.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(CommonObjs.Inst.MainCam, gun.SpriteRoot.transform, transform);
    }

    private void OnAnimStart_SwapMagazine()
    {
        AnimStart_SwapMagazine = true;
        AnimEnd_SwapMagazine = false;
    }
    private void OnAnimEnd_SwapMagazine()
    {
        AnimStart_SwapMagazine = false;
        AnimEnd_SwapMagazine = true;
    }
    private void DropMagazine()
    {
        ObjPoolingManager.Activate(droppedMagazinePrefab, magazineDropPos.position, transform.rotation);
    }
}

using UnityEngine;

public class GunSwapMagazine_Base<GunMain> : GunAction_Base<GunMain> 
    where GunMain : Gun
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


    #region Method: CLA_Action
    public override void OnEnter()
    {
        // Start Timer
        gun.gunData.swapMagazineTimer.SetActive(true);
        gun.gunData.swapMagazineTimer.ToZero();
        gun.gunData.swapMagazineTimer.Restart();

        IsAnimStarted_SwapMagazine = true;
        IsAnimEnded_SwapMagazine = false;

        // Animation
        animator.Play(gun.WeaponNameTrimed + "_SwapMagazine", 0, 0);
    }
    public override void OnLateEnter()
    {
        Anim_Logic.SetAnimSpeed(animator, gun.gunData.swapMagazineTimer.EndTime, gun.WeaponNameTrimed + "_SwapMagazine");
    }
    public override void OnExit()
    {
        // Stop Timer
        gun.gunData.swapMagazineTimer.SetActive(false);

        // On Timer End
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
    public override void OnLateUpdate()
    {
        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, gun.SpriteRoot.transform, transform);
    }
    #endregion

    #region Method: AnimEvent
    protected virtual void DropMagazine()
    {
        ObjPoolingManager.Activate(droppedMagazinePrefab, magazineDropPos.position, transform.rotation);
    }
    #endregion
}

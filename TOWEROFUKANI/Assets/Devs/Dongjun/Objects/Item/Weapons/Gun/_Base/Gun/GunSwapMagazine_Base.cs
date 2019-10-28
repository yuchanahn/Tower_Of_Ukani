using UnityEngine;

public abstract class GunSwapMagazine_Base<TItem> : GunAction_Base<TItem> 
    where TItem : GunItem
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
        weapon.swapMagazineTimer.SetActive(true);
        weapon.swapMagazineTimer.Restart();

        IsAnimStarted_SwapMagazine = true;
        IsAnimEnded_SwapMagazine = false;

        // Animation
        animator.Play(weapon.Info.NameTrimed + "_SwapMagazine", 0, 0);
    }
    public override void OnLateEnter()
    {
        Anim_Logic.SetAnimSpeed(animator, weapon.swapMagazineTimer.EndTime.Value, weapon.Info.NameTrimed + "_SwapMagazine");
    }
    public override void OnExit()
    {
        // Stop Timer
        weapon.swapMagazineTimer.SetActive(false);

        // On Timer End
        if (weapon.swapMagazineTimer.IsEnded)
        {
            IsAnimStarted_SwapMagazine = false;
            IsAnimEnded_SwapMagazine = true;

            // Load Bullets
            weapon.loadedBullets = reloadAll ? weapon.magazineSize.Value : weapon.loadedBullets + reloadAmount;
        }

        // Animation
        animator.speed = 1;
        animator.Play(weapon.Info.NameTrimed + "_Idle");
    }
    public override void OnLateUpdate()
    {
        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, weapon.SpriteRoot.transform, transform);
    }
    #endregion

    #region Method: AnimEvent
    protected virtual void DropMagazine()
    {
        ObjPoolingManager.Spawn(droppedMagazinePrefab, magazineDropPos.position, transform.rotation);
    }
    #endregion
}

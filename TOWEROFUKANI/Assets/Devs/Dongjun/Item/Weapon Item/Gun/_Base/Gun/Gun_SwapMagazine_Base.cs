using Dongjun.Helper;
using UnityEngine;

public abstract class Gun_SwapMagazine_Base<T> : Gun_State_Base<T> 
    where T : GunItem
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

    #region Method: SSM
    public override void OnEnter()
    {
        // Start Timer
        weapon.swapMagazineTimer.SetActive(true);
        weapon.swapMagazineTimer.Restart();

        IsAnimStarted_SwapMagazine = true;
        IsAnimEnded_SwapMagazine = false;

        // Animation
        weapon.animator.Play(weapon.ANIM_SwapMagazine, 0, 0);
    }
    public override void OnLateEnter()
    {
        weapon.animator.SetDuration(weapon.swapMagazineTimer.EndTime.Value, weapon.ANIM_SwapMagazine);
    }
    public override void OnExit()
    {
        // Stop Timer
        weapon.swapMagazineTimer.SetActive(false);
        weapon.swapMagazineTimer.ToZero();

        // On Timer End
        if (weapon.swapMagazineTimer.IsEnded)
        {
            IsAnimStarted_SwapMagazine = false;
            IsAnimEnded_SwapMagazine = true;

            // Load Bullets
            weapon.loadedBullets = reloadAll ? weapon.magazineSize.Value : weapon.loadedBullets + reloadAmount;
        }

        // Animation
        weapon.animator.ResetSpeed();
    }
    public override void OnLateUpdate()
    {
        // Look At Mouse
        transform.AimMouse(Global.Inst.MainCam, transform);
    }
    #endregion

    #region Method: AnimEvent
    protected virtual void AniEvent_DropMagazine()
    {
        ObjPoolingManager.Spawn(droppedMagazinePrefab, magazineDropPos.position, transform.rotation);
    }
    #endregion
}

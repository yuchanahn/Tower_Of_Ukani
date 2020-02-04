using UnityEngine;

public abstract class Gun_Reload_Base<T> : Gun_State_Base<T>
    where T : GunItem
{
    #region Var: Inspector
    [Header("Ammo")]
    [SerializeField] protected bool reloadAll = true;
    [SerializeField] protected int reloadAmount;
    #endregion

    #region Method: SSM
    public override void OnEnter()
    {
        // Start Timer
        weapon.reloadTimer.SetActive(true);
        weapon.reloadTimer.Restart();

        // Animation
        weapon.animator.Play(weapon.ANIM_Reload, 0, 0);
    }
    public override void OnLateEnter()
    {
        // Set Animation Speed
        weapon.animator.SetSpeed(weapon.reloadTimer.EndTime.Value, weapon.ANIM_Reload);
    }
    public override void OnExit()
    {
        // Stop Timer
        weapon.reloadTimer.SetActive(false);
        weapon.reloadTimer.ToZero();

        // Load Bullets
        if (weapon.reloadTimer.IsEnded)
        {
            weapon.loadedBullets = reloadAll ? weapon.magazineSize.Value : weapon.loadedBullets + reloadAmount;
            weapon.isBulletLoaded = true;
        }

        // Animation
        weapon.animator.ResetSpeed();
    }
    public override void OnLateUpdate()
    {
        if (!weapon.IsSelected)
            return;

        // Look At Mouse
        transform.AimMouse(Global.Inst.MainCam, transform);
    }
    #endregion
}

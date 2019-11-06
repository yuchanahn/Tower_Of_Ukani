using UnityEngine;

public abstract class GunReload_Base<TItem> : GunAction_Base<TItem> 
    where TItem : GunItem
{
    #region Var: Inspector
    [Header("Ammo")]
    [SerializeField] protected bool reloadAll = true;
    [SerializeField] protected int reloadAmount;
    #endregion

    #region Method: CLA_Action
    public override void OnEnter()
    {
        // Start Timer
        weapon.reloadTimer.SetActive(true);
        weapon.reloadTimer.Restart();

        // Animation
        animator.Play(weapon.Info.NameTrimed + "_Reload", 0, 0);
    }
    public override void OnLateEnter()
    {
        // Set Animation Speed
        Anim_Logic.SetAnimSpeed(animator, weapon.reloadTimer.EndTime.Value, weapon.Info.NameTrimed + "_Reload");
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
        animator.speed = 1;
        animator.Play(weapon.Info.NameTrimed + "_Idle");
    }
    public override void OnLateUpdate()
    {
        if (!weapon.IsSelected)
            return;

        // Look At Mouse
        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, weapon.SpriteRoot.transform, transform);
    }
    #endregion
}

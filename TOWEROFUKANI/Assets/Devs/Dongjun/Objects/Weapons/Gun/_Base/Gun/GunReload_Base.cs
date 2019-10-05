using UnityEngine;

public abstract class GunReload_Base<TMain> : GunAction_Base<TMain> 
    where TMain : Gun
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
        gun.reloadTimer.SetActive(true);
        gun.reloadTimer.Restart();

        // Animation
        animator.Play(gun.WeaponNameTrimed + "_Reload", 0, 0);
    }
    public override void OnLateEnter()
    {
        // Set Animation Speed
        Anim_Logic.SetAnimSpeed(animator, gun.reloadTimer.EndTime, gun.WeaponNameTrimed + "_Reload");
    }
    public override void OnExit()
    {
        // Stop Timer
        gun.reloadTimer.SetActive(false);

        // Load Bullets
        if (gun.reloadTimer.IsEnded)
        {
            gun.loadedBullets = reloadAll ? gun.magazineSize : gun.loadedBullets + reloadAmount;
            gun.isBulletLoaded = true;
        }

        // Animation
        animator.speed = 1;
        animator.Play(gun.WeaponNameTrimed + "_Idle");
    }
    public override void OnLateUpdate()
    {
        if (!gun.IsSelected)
            return;

        // Look At Mouse
        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, gun.SpriteRoot.transform, transform);
    }
    #endregion
}

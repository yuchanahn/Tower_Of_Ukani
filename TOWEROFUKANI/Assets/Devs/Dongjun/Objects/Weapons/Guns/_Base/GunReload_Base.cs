using UnityEngine;

public class GunReload_Base<GunMain> : GunAction_Base<GunMain> 
    where GunMain : Gun
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
        gun.gunData.reloadTimer.SetActive(true);
        gun.gunData.reloadTimer.ToZero();
        gun.gunData.reloadTimer.Restart();

        // Animation
        animator.Play(gun.WeaponNameTrimed + "_Reload", 0, 0);
    }
    public override void OnLateEnter()
    {
        // Set Animation Speed
        Anim_Logic.SetAnimSpeed(animator, gun.gunData.reloadTimer.EndTime, gun.WeaponNameTrimed + "_Reload");
    }
    public override void OnExit()
    {
        // Stop Timer
        gun.gunData.reloadTimer.SetActive(false);

        // Load Bullets
        if (gun.gunData.reloadTimer.IsEnded)
        {
            gun.gunData.loadedBullets = reloadAll ? gun.gunData.magazineSize : gun.gunData.loadedBullets + reloadAmount;
            gun.gunData.isBulletLoaded = true;
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

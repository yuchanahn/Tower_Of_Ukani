using UnityEngine;

public class Gun_Reload_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Ammo")]
    [SerializeField] protected bool reloadAll = true;
    [SerializeField] protected int reloadAmount;
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
        gun.gunData.reloadTimer.SetActive(true);
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
        // Look At Mouse
        LookAtMouse_Logic.Rotate(Global.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(Global.Inst.MainCam, gun.SpriteRoot.transform, transform);
    }
    #endregion
}

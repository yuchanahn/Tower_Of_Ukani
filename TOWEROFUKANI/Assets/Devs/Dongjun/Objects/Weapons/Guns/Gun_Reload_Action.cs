using UnityEngine;

public class Gun_Reload_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Ammo")]
    [SerializeField] private bool reloadAll = true;
    [SerializeField] private int reloadAmount;
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
        gun.Stats.reloadTimer.UseAutoTick(true);
        gun.Stats.reloadTimer.Restart();

        // Animation
        animator.Play(gun.WeaponNameTrimed + "_Reload", 0, 0);
    }
    public override void OnStart()
    {
        // Set Animation Speed
        AnimSpeed_Logic.SetAnimSpeed(animator, gun.Stats.reloadTimer.Timer_Max, gun.WeaponNameTrimed + "_Reload");
    }
    public override void OnEnd()
    {
        // Stop Timer
        gun.Stats.reloadTimer.UseAutoTick(false);

        // Load Bullets
        if (gun.Stats.reloadTimer.IsTimerAtMax)
            gun.Stats.loadedBullets = reloadAll ? gun.Stats.magazineSize : gun.Stats.loadedBullets + reloadAmount;

        // Animation
        animator.speed = 1;
        animator.Play(gun.WeaponNameTrimed + "_Idle");
    }
    public override void OnLateUpdate()
    {
        // Rotate And Filp
        LookAtMouse_Logic.Rotate(Global.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(Global.Inst.MainCam, gun.SpriteRoot.transform, transform);
    }
    #endregion
}

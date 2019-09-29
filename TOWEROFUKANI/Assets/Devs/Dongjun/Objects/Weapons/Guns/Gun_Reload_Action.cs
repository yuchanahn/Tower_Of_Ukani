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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gun = GetComponent<Gun>();
    }

    public override void OnStart()
    {
        // Start Timer
        gun.Stats.reloadTimer.Timer_Cur = 0;
        gun.Stats.reloadTimer.SetActive(true);
        gun.Stats.reloadTimer.Continue();

        // Animation
        animator.Play(gun.WeaponName + "_Reload", 0, 0);
    }
    public override void OnEnd()
    {
        // Stop Timer
        gun.Stats.reloadTimer.SetActive(false);

        // Load Bullets
        if (gun.Stats.reloadTimer.IsTimerAtMax)
            gun.Stats.loadedBullets = reloadAll ? gun.Stats.magazineSize : gun.Stats.loadedBullets + reloadAmount;

        // Animation
        animator.speed = 1;
        animator.Play(gun.WeaponName + "_Idle");
    }
    public override void OnLateUpdate()
    {
        AnimSpeed_Logic.SetAnimSpeed(animator, gun.Stats.reloadTimer.Timer_Max, gun.WeaponName + "_Reload");
        LookAtMouse_Logic.Rotate(CommonObjs.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(CommonObjs.Inst.MainCam, gun.SpriteRoot.transform, transform);
    }
}

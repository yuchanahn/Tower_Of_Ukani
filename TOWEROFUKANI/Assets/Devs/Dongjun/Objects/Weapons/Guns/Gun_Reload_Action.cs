using UnityEngine;

public class Gun_Reload_Action : CLA_Action
{
    #region Var: Components
    private Animator animator;
    private Gun gun_Main;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gun_Main = GetComponent<Gun>();
    }

    public override void OnStart()
    {
        gun_Main.Stats.reloadTimer.Timer_Cur = 0;
        gun_Main.Stats.reloadTimer.SetActive(true);
        gun_Main.Stats.reloadTimer.Continue();

        animator.ResetTrigger("Shoot");
        animator.Play("Pistol_Reload", 0, 0);
    }
    public override void OnEnd()
    {
        gun_Main.Stats.reloadTimer.SetActive(false);
        if (gun_Main.Stats.reloadTimer.IsTimerAtMax)
            gun_Main.Stats.loadedBullets = gun_Main.Stats.magazineSize;

        animator.speed = 1;
        animator.Play("Pistol_Idle");
    }
    public override void OnLateUpdate()
    {
        AnimSpeed_Logic.SetAnimSpeed(animator, gun_Main.Stats.reloadTimer.Timer_Max, "Pistol_Reload");
        LookAtMouse_Logic.Rotate(CommonObjs.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(CommonObjs.Inst.MainCam, gun_Main.SpriteRoot.transform, transform);
    }
}

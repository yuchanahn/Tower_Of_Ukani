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
        animator.ResetTrigger("Shoot");
        animator.Play("Pistol_Reload");

        gun_Main.Stats.reloadTimer.Timer_Cur = 0;
        gun_Main.Stats.reloadTimer.Continue();
    }
    public override void OnEnd()
    {
        animator.Play("Pistol_Idle");
        gun_Main.Stats.loadedBullets = gun_Main.Stats.magazineSize;
    }
    public override void OnLateUpdate()
    {
        LookAtMouse_Logic.Rotate(CommonObjs.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(CommonObjs.Inst.MainCam, gun_Main.SpriteRoot.transform, transform);
    }
}

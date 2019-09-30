using UnityEngine;

public class Shotgun : Gun
{
    #region Var: CLA_Action
    private Shotgun_Main_Action main_AC;
    private Gun_Reload_Action reload_AC;
    private Gun_SwapMagazine_Action swapMagazine_AC;
    #endregion


    #region Method: Init CLA_Main
    protected override void Init()
    {
        main_AC = GetComponent<Shotgun_Main_Action>();
        reload_AC = GetComponent<Gun_Reload_Action>();
        swapMagazine_AC = GetComponent<Gun_SwapMagazine_Action>();

        ConditionLogics.Add(main_AC, CL_Main_AC);
        ConditionLogics.Add(reload_AC, CL_Reload_AC);
        ConditionLogics.Add(swapMagazine_AC, CL_SwapMagazine_AC);
    }
    #endregion

    #region Method: Unity
    protected override void Start()
    {
        base.Start();

        // Init Timer
        gunData.shootTimer.Init(gameObject);
        gunData.reloadTimer.Init(gameObject);
        gunData.swapMagazineTimer.Init(gameObject);

        // Init Ammo
        gunData.loadedBullets = gunData.magazineSize;
    }
    #endregion

    #region Method: Condition Logic
    private bool CL_Gun()
    {
        if (!IsSelected)
        { ChangeAction(main_AC); return true; }

        return false;
    }
    private void CL_Main_AC()
    {
        if (CL_Gun()) return;

        if (!gunData.isBulletLoaded && gunData.shootTimer.IsTimerAtMax && gunData.loadedBullets > 0)
        { ChangeAction(reload_AC);  return; }

        if (gunData.loadedBullets <= 0)
        {
            if (gunData.shootTimer.IsTimerAtMax)
            { ChangeAction(swapMagazine_AC); return; }

            if (swapMagazine_AC.AnimStart_SwapMagazine && !swapMagazine_AC.AnimEnd_SwapMagazine)
            { ChangeAction(swapMagazine_AC); return; }
        }

        if (gunData.loadedBullets < gunData.magazineSize && Input.GetKeyDown(PlayerInputManager.Inst.Keys.Reload))
        { ChangeAction(swapMagazine_AC); return; }
    }
    private void CL_Reload_AC()
    {
        if (CL_Gun()) return;

        if (gunData.reloadTimer.IsTimerAtMax)
        { ChangeAction(main_AC); return; }
    }
    private void CL_SwapMagazine_AC()
    {
        if (CL_Gun()) return;

        if (gunData.swapMagazineTimer.IsTimerAtMax)
        { ChangeAction(reload_AC); return; }
    }
    #endregion
}

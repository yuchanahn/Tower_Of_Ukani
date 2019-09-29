using UnityEngine;

public class MachineGun : Gun
{
    #region Var: CLA_Action
    private MachineGun_Main_Action main_AC;
    private Gun_Reload_Action reload_AC;
    private Gun_SwapMagazine_Action swapMagazine_AC;
    #endregion


    #region Method: Init CLA_Main
    protected override void Init()
    {
        main_AC = GetComponent<MachineGun_Main_Action>();
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
        Stats.shootTimer.Init(gameObject);
        Stats.reloadTimer.Init(gameObject);
        Stats.swapMagazineTimer.Init(gameObject);

        // Init Ammo
        Stats.loadedBullets = Stats.magazineSize;
    }
    #endregion

    #region Method: Condition Logic
    private bool CL_Gun()
    {
        if (!IsSelected)
        { ChangeAction(main_AC); return true; }

        //if (CurrentAction == reload_AC && Stats.reloadTimer.IsTimerAtMax)
        //{ ChangeAction(main_AC); return; }

        //if (CurrentAction == swapMagazine_AC && Stats.swapMagazineTimer.IsTimerAtMax)
        //{ ChangeAction(reload_AC); return; }

        //if (CurrentAction == main_AC && Stats.loadedBullets <= 0)
        //{
        //    if (main_AC.AnimEnd_Shoot)
        //    { ChangeAction(swapMagazine_AC); return; }

        //    if (swapMagazine_AC.AnimStart_SwapMagazine && !swapMagazine_AC.AnimEnd_SwapMagazine)
        //    { ChangeAction(swapMagazine_AC); return; }
        //}

        return false;
    }
    private void CL_Main_AC()
    {
        if (CL_Gun()) return;

        if (Stats.loadedBullets <= 0)
        {
            if (main_AC.AnimEnd_Shoot)
            { ChangeAction(swapMagazine_AC); return; }

            if (swapMagazine_AC.AnimStart_SwapMagazine && !swapMagazine_AC.AnimEnd_SwapMagazine)
            { ChangeAction(swapMagazine_AC); return; }

            if (Stats.loadedBullets < Stats.magazineSize && Input.GetKeyDown(PlayerInputManager.Inst.Keys.Reload))
            { ChangeAction(swapMagazine_AC); return; }
        }
    }
    private void CL_Reload_AC()
    {
        if (CL_Gun()) return;

        if (Stats.reloadTimer.IsTimerAtMax)
        { ChangeAction(main_AC); return; }
    }
    private void CL_SwapMagazine_AC()
    {
        if (CL_Gun()) return;

        if (Stats.swapMagazineTimer.IsTimerAtMax)
        { ChangeAction(reload_AC); return; }
    }
    #endregion
}

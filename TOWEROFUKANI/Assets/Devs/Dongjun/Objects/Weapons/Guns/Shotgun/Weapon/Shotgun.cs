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

    #region Method: Condition Logic
    private void CL_Main_AC()
    {
        if (CL_NotSelected())
            return;

        if (!gunData.isBulletLoaded && main_AC.IsAnimEnded_Shoot && gunData.loadedBullets > 0)
        {
            ChangeAction(reload_AC);
        }
        else if (gunData.loadedBullets <= 0)
        {
            if (main_AC.IsAnimEnded_Shoot)
                ChangeAction(swapMagazine_AC);
            else if (swapMagazine_AC.IsAnimStarted_SwapMagazine && !swapMagazine_AC.IsAnimEnded_SwapMagazine)
                ChangeAction(swapMagazine_AC);
        }
        else
        {
            if (Input.GetKeyDown(PlayerInputManager.Inst.Keys.Reload))
                ChangeAction(swapMagazine_AC);
        }
    }
    private void CL_Reload_AC()
    {
        if (CL_NotSelected())
            return;

        if (gunData.reloadTimer.IsEnded)
            ChangeAction(main_AC);
    }
    private void CL_SwapMagazine_AC()
    {
        if (CL_NotSelected())
            return;

        if (gunData.swapMagazineTimer.IsEnded)
            ChangeAction(reload_AC);
    }
    #endregion
}

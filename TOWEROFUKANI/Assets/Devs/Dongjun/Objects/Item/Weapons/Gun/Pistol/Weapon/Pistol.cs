using UnityEngine;

public class Pistol : GunController<PistolItem>
{
    #region Var: CLA_Action
    private Pistol_Main_Action action_Main;
    private Gun_Reload_Action action_Reload;
    private Gun_SwapMagazine_Action action_SwapMagazine;
    #endregion

    #region Method: Init
    protected override void Init()
    {
        AddLogic(ref action_Main, CL_Main);
        AddLogic(ref action_Reload, CL_Reload);
        AddLogic(ref action_SwapMagazine, CL_SwapMagazine);
    }
    #endregion

    #region Method: Condition Logic
    private CLA_Action_Base CL_Main()
    {
        if (!weaponItem.IsSelected)
            return DefaultAction;

        if (weaponItem.loadedBullets <= 0)
        {
            if (action_SwapMagazine.IsAnimEnded_SwapMagazine && !weaponItem.reloadTimer.IsEnded)
                return action_Reload;

            if (action_Main.IsAnimEnded_Shoot)
                return action_SwapMagazine;

            if (action_SwapMagazine.IsAnimStarted_SwapMagazine && !action_SwapMagazine.IsAnimEnded_SwapMagazine)
                return action_SwapMagazine;
        }
        else if (weaponItem.loadedBullets < weaponItem.magazineSize.Value)
        {
            if (Input.GetKeyDown(PlayerWeaponKeys.Reload))
                return action_SwapMagazine;
        }

        return null;
    }
    private CLA_Action_Base CL_Reload()
    {
        if (!weaponItem.IsSelected)
            return DefaultAction;

        if (weaponItem.reloadTimer.IsEnded)
            return action_Main;

        return null;
    }
    private CLA_Action_Base CL_SwapMagazine()
    {
        if (!weaponItem.IsSelected)
            return DefaultAction;

        if (weaponItem.swapMagazineTimer.IsEnded)
            return action_Reload;

        return null;
    }
    #endregion
}

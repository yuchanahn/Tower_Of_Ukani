﻿using UnityEngine;

public class MachineGun : Gun
{
    #region Var: Inspector
    [Header("Ammo Belt")]
    public Transform ammoBelt;
    public float ammoBeltAmmoCount;
    #endregion

    #region Var: CLA_Action
    private MachineGun_Main_Action main_AC;
    private Gun_Reload_Action reload_AC;
    private MachineGun_SwapMagazine_Action swapMagazine_AC;
    #endregion

    #region Var: Properties
    public float AmmoBeltMaxY => 0.0625f * ammoBeltAmmoCount;
    #endregion


    #region Method: Init
    protected override void Init()
    {
        main_AC = GetComponent<MachineGun_Main_Action>();
        reload_AC = GetComponent<Gun_Reload_Action>();
        swapMagazine_AC = GetComponent<MachineGun_SwapMagazine_Action>();

        ConditionLogics.Add(main_AC, CL_Main);
        ConditionLogics.Add(reload_AC, CL_Reload);
        ConditionLogics.Add(swapMagazine_AC, CL_SwapMagazine);
    }
    #endregion

    #region Method: Condition Logic
    private CLA_Action CL_Main()
    {
        if (!IsSelected)
            return DefaultAction;

        if (loadedBullets <= 0)
        {
            if (swapMagazine_AC.IsAnimEnded_SwapMagazine && !reloadTimer.IsEnded)
                return reload_AC;

            if (main_AC.IsAnimEnded_Shoot)
                return swapMagazine_AC;

            if (swapMagazine_AC.IsAnimStarted_SwapMagazine && !swapMagazine_AC.IsAnimEnded_SwapMagazine)
                return swapMagazine_AC;
        }
        else if (loadedBullets < magazineSize)
        {
            if (Input.GetKeyDown(PlayerWeaponKeys.Reload))
                return swapMagazine_AC;
        }

        return main_AC;
    }
    private CLA_Action CL_Reload()
    {
        if (!IsSelected)
            return DefaultAction;

        if (reloadTimer.IsEnded)
            return main_AC;

        return reload_AC;
    }
    private CLA_Action CL_SwapMagazine()
    {
        if (!IsSelected)
            return DefaultAction;

        if (swapMagazineTimer.IsEnded)
            return reload_AC;

        return swapMagazine_AC;
    }
    #endregion
}
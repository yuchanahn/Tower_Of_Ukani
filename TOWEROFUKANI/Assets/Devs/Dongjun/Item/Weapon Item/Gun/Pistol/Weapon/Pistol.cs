using UnityEngine;

public class Pistol : GunController<PistolItem>
{
    #region Var: States
    private Pistol_Main state_Main;
    private Gun_Reload state_Reload;
    private Gun_SwapMagazine state_SwapMagazine;
    #endregion

    #region Method: Init
    protected override void InitStates()
    {
        SetLogic(When.AnyAction, () =>
        {
            if (PlayerStatus.IsHardCCed)
                return state_Main;

            return null;
        });

        SetLogic(ref state_Main, () =>
        {
            if (!weaponItem.IsSelected)
                return DefaultState;

            if (weaponItem.loadedBullets <= 0)
            {
                if (state_SwapMagazine.IsAnimEnded_SwapMagazine && !weaponItem.reloadTimer.IsEnded)
                    return state_Reload;

                if (state_Main.IsAnimEnded_Shoot)
                    return state_SwapMagazine;

                if (state_SwapMagazine.IsAnimStarted_SwapMagazine && !state_SwapMagazine.IsAnimEnded_SwapMagazine)
                    return state_SwapMagazine;
            }
            else if (weaponItem.loadedBullets < weaponItem.magazineSize.Value)
            {
                if (PlayerWeaponKeys.GetKeyDown(PlayerWeaponKeys.Reload))
                    return state_SwapMagazine;
            }

            return null;
        });

        SetLogic(ref state_Reload, () =>
        {
            if (!weaponItem.IsSelected)
                return DefaultState;

            if (weaponItem.reloadTimer.IsEnded)
                return state_Main;

            return null;
        });

        SetLogic(ref state_SwapMagazine, () =>
        {
            if (!weaponItem.IsSelected)
                return DefaultState;

            if (weaponItem.swapMagazineTimer.IsEnded)
                return state_Reload;

            return null;
        });

        SetDefaultState(state_Main);
    }
    #endregion
}

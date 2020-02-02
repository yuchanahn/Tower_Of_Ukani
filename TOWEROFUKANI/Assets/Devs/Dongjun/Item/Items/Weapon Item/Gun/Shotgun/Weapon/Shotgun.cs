using UnityEngine;

public class Shotgun : GunController<ShotgunItem>
{
    #region Var: States
    private Shotgun_Main state_Main;
    private Gun_Reload state_Reload;
    private Gun_SwapMagazine state_SwapMagazine;
    #endregion

    #region Method: Init
    protected override void InitStates()
    {
        SetLogic(ref state_Main, () =>
        {
            if (!weaponItem.IsSelected)
                return state_Main;

            if (!weaponItem.isBulletLoaded && state_Main.IsAnimEnded_Shoot && weaponItem.loadedBullets > 0)
                return state_Reload;

            if (weaponItem.loadedBullets <= 0)
            {
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
                return state_Main;

            if (weaponItem.reloadTimer.IsEnded)
                return state_Main;

            return null;
        });

        SetLogic(ref state_SwapMagazine, () =>
        {
            if (!weaponItem.IsSelected)
                return state_Main;

            if (weaponItem.swapMagazineTimer.IsEnded)
                return state_Reload;

            return null;
        });

        SetDefaultState(state_Main);
    }
    #endregion
}

using UnityEngine;

public class MachineGun : Gun
{
    #region Var: Inspector
    [Header("Ammo Belt")]
    [SerializeField] public Transform ammoBelt;
    [SerializeField] public float ammoBeltAmmoCount;
    #endregion

    #region Var: CLA_Action
    private MachineGun_Main_Action main_AC;
    private Gun_Reload_Action reload_AC;
    private MachineGun_SwapMagazine_Action swapMagazine_AC;
    #endregion


    #region Method: Init CLA_Main
    protected override void Init()
    {
        main_AC = GetComponent<MachineGun_Main_Action>();
        reload_AC = GetComponent<Gun_Reload_Action>();
        swapMagazine_AC = GetComponent<MachineGun_SwapMagazine_Action>();

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
    private bool CL_Base()
    {
        if (!IsSelected)
        { ChangeAction(main_AC); return true; }

        return false;
    }
    private void CL_Main_AC()
    {
        if (CL_Base()) return;

        if (gunData.loadedBullets <= 0)
        {
            if (swapMagazine_AC.IsAnimEnded_SwapMagazine && !gunData.reloadTimer.IsEnded)
            { ChangeAction(reload_AC); return; }

            if (main_AC.IsAnimEnded_Shoot)
            { ChangeAction(swapMagazine_AC); return; }

            if (swapMagazine_AC.IsAnimStarted_SwapMagazine && !swapMagazine_AC.IsAnimEnded_SwapMagazine)
            { ChangeAction(swapMagazine_AC); return; }
        }

        if (gunData.loadedBullets < gunData.magazineSize && Input.GetKeyDown(PlayerInputManager.Inst.Keys.Reload))
        { ChangeAction(swapMagazine_AC); return; }
    }
    private void CL_Reload_AC()
    {
        if (CL_Base()) return;

        if (gunData.reloadTimer.IsEnded)
        { ChangeAction(main_AC); return; }
    }
    private void CL_SwapMagazine_AC()
    {
        if (CL_Base()) return;

        if (gunData.swapMagazineTimer.IsEnded)
        { ChangeAction(reload_AC); return; }
    }
    #endregion
}

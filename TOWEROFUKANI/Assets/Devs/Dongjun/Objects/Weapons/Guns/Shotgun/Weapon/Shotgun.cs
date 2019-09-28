﻿using UnityEngine;

public class Shotgun : Gun
{
    private Shotgun_Main_Action main_AC;
    private Gun_Reload_Action reload_AC;
    private Gun_SwapMagazine_Action swapMagazine_AC;


    protected override void Init()
    {
        main_AC = GetComponent<Shotgun_Main_Action>();
        reload_AC = GetComponent<Gun_Reload_Action>();
        swapMagazine_AC = GetComponent<Gun_SwapMagazine_Action>();

        ConditionLogics.Add(main_AC, CL_Gun);
        ConditionLogics.Add(reload_AC, CL_Gun);
        ConditionLogics.Add(swapMagazine_AC, CL_Gun);
    }
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

    private void CL_Gun()
    {
        if (!IsSelected)
        { ChangeAction(main_AC); return; }

        if (CurrentAction == reload_AC && Stats.reloadTimer.IsTimerAtMax)
        { ChangeAction(main_AC); return; }

        if (CurrentAction == swapMagazine_AC && Stats.swapMagazineTimer.IsTimerAtMax)
        { ChangeAction(reload_AC); return; }

        if (CurrentAction == main_AC && Stats.loadedBullets <= 0)
        {
            if (main_AC.AnimEnd_Shoot)
            { ChangeAction(swapMagazine_AC); return; }

            if (swapMagazine_AC.AnimStart_SwapMagazine && !swapMagazine_AC.AnimEnd_SwapMagazine)
            { ChangeAction(swapMagazine_AC); return; }
        }

        if (CurrentAction == main_AC && main_AC.AnimEnd_Shoot && Stats.loadedBullets > 0)
        { ChangeAction(reload_AC); return; }

        if (Stats.loadedBullets < Stats.magazineSize && Input.GetKeyDown(PlayerInputManager.Inst.Keys.Reload))
        { ChangeAction(swapMagazine_AC); return; }
    }
}

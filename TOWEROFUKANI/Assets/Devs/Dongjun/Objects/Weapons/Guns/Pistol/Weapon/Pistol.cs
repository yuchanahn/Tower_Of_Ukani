using UnityEngine;

public class Pistol : Gun
{
    private Pistol_Main_Action main_Action;
    private Gun_Reload_Action reload_Action;


    protected override void Init()
    {
        main_Action = GetComponent<Pistol_Main_Action>();
        reload_Action = GetComponent<Gun_Reload_Action>();

        ConditionLogics.Add(main_Action, CL_Gun);
        ConditionLogics.Add(reload_Action, CL_Gun);
    }
    protected override void Start()
    {
        base.Start();
        Stats.shootTimer.Init(gameObject);
        Stats.reloadTimer.Init(gameObject);

        Stats.loadedBullets = Stats.magazineSize;
    }

    private void CL_Gun()
    {
        if (CurrentAction == reload_Action && Stats.reloadTimer.IsTimerAtMax)
        {
            ChangeAction(main_Action);
            return;
        }

        if (CurrentAction == main_Action && Stats.loadedBullets <= 0)
        {
            ChangeAction(reload_Action);
            return;
        }

        if (CurrentAction != reload_Action &&
            Stats.loadedBullets < Stats.magazineSize && 
            Input.GetKeyDown(PlayerInputManager.Inst.Keys.Reload))
        {
            ChangeAction(reload_Action);
            return;
        }
    }
}

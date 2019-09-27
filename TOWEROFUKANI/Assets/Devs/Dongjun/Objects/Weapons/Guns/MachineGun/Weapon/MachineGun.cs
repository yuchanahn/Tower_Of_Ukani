using UnityEngine;

public class MachineGun : Gun
{
    private MachineGun_Main_Action machineGun_Main_Action;

    protected override void Init()
    {
        machineGun_Main_Action = GetComponent<MachineGun_Main_Action>();

        ConditionLogics.Add(machineGun_Main_Action, CL_MainAction);
    }

    private void CL_MainAction()
    {

    }
}

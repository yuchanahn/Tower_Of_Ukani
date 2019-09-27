using UnityEngine;

public class Shotgun : Gun
{
    private Shotgun_Main_Action pistol_Main_Action;

    protected override void Init()
    {
        pistol_Main_Action = GetComponent<Shotgun_Main_Action>();

        ConditionLogics.Add(pistol_Main_Action, CL_MainAction);
    }

    private void CL_MainAction()
    {
        // Write Condition Logic Here
    }
}

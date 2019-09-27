using UnityEngine;

public class Pistol : Gun
{
    private Pistol_Main_Action pistol_Main_Action;

    protected override void Init()
    {
        pistol_Main_Action = GetComponent<Pistol_Main_Action>();

        ConditionLogics.Add(pistol_Main_Action, CL_MainAction);
    }

    private void CL_MainAction()
    {
        // Write Condition Logic Here
    }
}

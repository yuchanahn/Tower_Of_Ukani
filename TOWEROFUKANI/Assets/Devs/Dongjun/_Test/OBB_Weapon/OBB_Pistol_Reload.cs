using Dongjun.Helper;
using UnityEngine;

public class OBB_Pistol_Reload : OBB_State<OBB_Pistol_Data>
{
    public override void OnEnter()
    {
        data.Timer_Reload.SetActive(true);

        data.Animator.Play("Reload", 0, 0f);
    }
    public override void OnLateEnter()
    {
        data.Animator.SetDuration(data.Timer_Reload.EndTime.Value);
    }
    public override void OnExit()
    {
        if (data.Timer_Reload.IsEnded)
            data.loadedBullets = data.magazineSize;

        data.Timer_Reload.SetActive(false);
        data.Timer_Reload.Restart();

        data.Animator.ResetSpeed();
    }
}

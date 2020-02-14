using Dongjun.Helper;
using UnityEngine;

public class OBB_Pistol_SwapMagazine : OBB_State<OBB_Pistol_Data>
{
    public override void OnEnter()
    {
        data.Timer_SwapMagazine.SetActive(true);

        data.Animator.Play("SwapMagazine", 0, 0f);
    }
    public override void OnLateEnter()
    {
        data.Animator.SetDuration(data.Timer_SwapMagazine.EndTime.Value);
    }
    public override void OnExit()
    {
        data.Timer_SwapMagazine.SetActive(false);
        data.Timer_SwapMagazine.Restart();

        data.Animator.ResetSpeed();
    }

    private void AniEvent_DropMagazine()
    {

    }
}

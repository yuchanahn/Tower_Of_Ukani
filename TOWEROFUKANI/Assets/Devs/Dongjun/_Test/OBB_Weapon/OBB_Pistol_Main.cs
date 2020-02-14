using Dongjun.Helper;
using UnityEngine;

public class OBB_Pistol_Main : OBB_State<OBB_Pistol_Data>
{
    public override void OnEnter()
    {
        data.loadedBullets--;

        data.Timer_Shoot.Restart();

        data.Animator.Play("Shoot");

        Debug.Log("Bang!");
    }
    public override void OnLateEnter()
    {
        data.Animator.SetDuration(data.Timer_Shoot.EndTime.Value);
    }
    public override void OnExit()
    {
        data.Animator.ResetSpeed();
    }

    private void OnAnimEnd_Shoot()
    {

    }
}

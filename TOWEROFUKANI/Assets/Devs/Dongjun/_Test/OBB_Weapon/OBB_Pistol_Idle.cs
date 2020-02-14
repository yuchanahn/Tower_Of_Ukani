using UnityEngine;

public class OBB_Pistol_Idle : OBB_State<OBB_Pistol_Data>
{
    public override void OnEnter()
    {
        data.Animator.Play("Idle");

        Debug.Log("Idle!");
    }
}

using UnityEngine;

public class Fist_Dash : Melee_State_Base<FistItem>
{
    public override void OnEnter()
    {
        // Look Dir
        Vector3 lookRot = transform.localEulerAngles;
        lookRot.y = GM.Player.Dir == 1 ? 0f : 180f;
        transform.localRotation = Quaternion.Euler(lookRot);
    }
    public override void OnUpdate()
    {
        weapon.animator.Play("Dash");
    }
}

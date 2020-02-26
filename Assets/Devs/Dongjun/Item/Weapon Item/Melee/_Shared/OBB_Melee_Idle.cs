using UnityEngine;

public class OBB_Melee_Idle : HorizontalWeapon_Idle_Base<OBB_Data_Animator, MeleeItem>
{
    public override void OnEnter()
    {
        data.Animator.Play(weaponItem.ANIM_Neutral);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_IceStaff_Idle : HorizontalWeapon_Idle_Base<OBB_IceStaff_Data, IceStaffItem>
{
    public override void OnEnter()
    {
        data.Animator.Play(weaponItem.ANIM_Idle);
    }
}

using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_IceStaff_Cast : HorizontalWeapon_State_Base<OBB_IceStaff_Data, IceStaffItem>
{
    public override void OnEnter()
    {
        data.Animator.Play("Cast");
    }
    public override void OnLateEnter()
    {
        if (weaponItem.Sub_CastDur.IsActive)
            data.Animator.SetDuration(weaponItem.Sub_CastDur.EndTime.Value);
        else if (weaponItem.Spec_CastDur.IsActive)
            data.Animator.SetDuration(weaponItem.Spec_CastDur.EndTime.Value);
    }
}

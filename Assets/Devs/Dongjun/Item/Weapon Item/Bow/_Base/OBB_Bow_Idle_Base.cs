using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_Bow_Idle_Base<D, W> : AimedWeapon_Idle_Base<D, W>
    where D : OBB_Data_Animator
    where W : BowItem
{
    public override void OnEnter()
    {
        data.Animator.Play(weaponItem.ANIM_Idle);
    }
}

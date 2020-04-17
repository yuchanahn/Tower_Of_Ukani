using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_Bow_Idle : AimedWeapon_Idle_Base<OBB_Data_Animator, BowItem>
{
    public override void OnEnter()
    {
        data.Animator.Play(weaponItem.ANIM_Idle);
    }
}

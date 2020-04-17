using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_IceStaff_Sub : Weapon_State_Base<OBB_IceStaff_Data, IceStaffItem>
{
    [SerializeField] private GameObject blizzardEffect;

    public override void OnEnter()
    {
        blizzardEffect.SetActive(true);
    }
    public override void OnExit()
    {
        blizzardEffect.SetActive(false);
    }
}

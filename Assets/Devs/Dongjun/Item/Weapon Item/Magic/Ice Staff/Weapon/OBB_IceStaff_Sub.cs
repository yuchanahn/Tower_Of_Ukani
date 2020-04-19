using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_IceStaff_Sub : Weapon_State_Base<OBB_IceStaff_Data, IceStaffItem>
{
    [SerializeField] private IceStaffBlizzard blizzardPrefab;
    private IceStaffBlizzard blizzard;

    public override void OnEnter()
    {
        if (data.IsBlizzardActive)
        {
            data.IsBlizzardActive = false;

            if (blizzard != null)
                blizzard.End();
        }
        else
        {
            data.IsBlizzardActive = true;

            if (blizzard == null)
                blizzard = Instantiate(blizzardPrefab.gameObject, GM.Player.transform, false).GetComponent<IceStaffBlizzard>();

            blizzard.Init(weaponItem, data);
        }
    }
}

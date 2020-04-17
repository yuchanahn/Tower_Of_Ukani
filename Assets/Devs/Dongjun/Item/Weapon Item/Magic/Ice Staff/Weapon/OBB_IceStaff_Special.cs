using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_IceStaff_Special : Weapon_State_Base<OBB_IceStaff_Data, IceStaffItem>
{
    [SerializeField] private IceBlock iceBlockPrefab;
    [SerializeField] private float shootPower;

    public override void OnEnter()
    {
        var iceBlock = iceBlockPrefab.Spawn(GM.PlayerPos);
        iceBlock.sleepTimer.EndTime = weaponItem.Spec_IceBlockDur.EndTime.Value;
        iceBlock.GetComponent<Rigidbody2D>().velocity = LookAtMouse_Logic.GetMouseDir(CamManager.Inst.MainCam, GM.PlayerPos) * shootPower;
    }
}

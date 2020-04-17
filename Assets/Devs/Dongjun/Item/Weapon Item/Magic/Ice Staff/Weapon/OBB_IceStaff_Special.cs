using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_IceStaff_Special : Weapon_State_Base<OBB_IceStaff_Data, IceStaffItem>
{
    [SerializeField] private GameObject iceBlockPrefab;
    [SerializeField] private float shootPower;

    public override void OnEnter()
    {
        GameObject iceBlock = Instantiate(iceBlockPrefab);
        iceBlock.transform.position = GM.PlayerPos;
        iceBlock.GetComponent<Rigidbody2D>().velocity = LookAtMouse_Logic.GetMouseDir(CamManager.Inst.MainCam, GM.PlayerPos) * shootPower;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Object : MonoBehaviour
{
    [SerializeField] Transform feetPos;
    [SerializeField] SpriteRenderer Pspr;
    void CreateAirJump(  )
    {
        AirJumpEft.Create(feetPos.position);
    }

    void CreateWalkDust(  )
    {
        var obj = PlayerWalkDustEft.Create(feetPos.position);
        var VelDir = Global.Inst.PlayerRB2D.velocity.x >= 0;

        obj.GetComponentInChildren<SpriteRenderer>().flipX = !((VelDir && !Pspr.flipX) || !(!VelDir && Pspr.flipX));
    }



}

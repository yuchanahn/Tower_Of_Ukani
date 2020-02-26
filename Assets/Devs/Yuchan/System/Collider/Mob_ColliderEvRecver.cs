using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_ColliderEvRecver : AColliderEvent
{
    public override void RecvEvent(Collider2D[] objs)
    {
        foreach(var i in objs)
        {
            if(i.tag == "Player")
            {
                PlayerStats.Inst.Damage(GetComponentInChildren<AStat>().Damage);
                GetComponent<Mob_Base>().OnSuccessfulAttack();
                GetComponentInChildren<AColliderMgr>().Stop();
            }
        }
    }
}

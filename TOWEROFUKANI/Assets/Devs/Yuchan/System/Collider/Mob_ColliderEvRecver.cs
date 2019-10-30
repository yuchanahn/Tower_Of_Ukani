﻿using System.Collections;
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
                PlayerStats.Damage(GetComponentInChildren<AStat>().Damage);
                GetComponentInChildren<AColliderMgr>().Stop();
            }
        }
    }
}
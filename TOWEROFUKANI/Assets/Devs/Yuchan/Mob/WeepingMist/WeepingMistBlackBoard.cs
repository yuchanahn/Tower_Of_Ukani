using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeepingMistBlackBoard : BlackBoard_Base
{
    protected Mob_WeepingMist mob;
    private void Awake()
    {
        mob = GetComponent<Mob_WeepingMist>();
    }

    public bool TA_RandomMove() => mob.RandomMove();
    public bool CN_Hurt()
    {
        return mob.IsHurting;
    }
    public bool TA_Hurt()
    {
        return true;
    }
}
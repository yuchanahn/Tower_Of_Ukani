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

    public bool CN_IsStund() => GetComponent<StatusEffect_Stunned>();

    // 스턴 상태 일경우 뭘 할껀지 아무것도 안하니까 True
    public bool TA_StundEvent() => true;

    public bool CN_IsPlayerInAgroRange() => Vector2.Distance(GM.PlayerPos, mob.transform.position) < mob.AgroRange;
    public bool TA_Follow() => mob.Follow();
}
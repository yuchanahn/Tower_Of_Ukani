using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStaffItem : MagicBase
{
    public readonly TimerStat Cooldown_Main = new TimerStat();

    // Bullet Data
    public ProjectileData BulletData
    { get; private set; }

    private void Start()
    {
        Cooldown_Main.SetTick(gameObject).ToEnd();
        Cooldown_Main.SetActive(true);
    }
    public override void InitStats()
    {
        AttackData = new AttackData(4);

        Cooldown_Main.EndTime = new FloatStat(0.6f, min: 0.01f);

        BulletData = new ProjectileData()
        {
            moveSpeed = new FloatStat(25f, min: 0f),
            travelDist = new FloatStat(0f, min: 0f, max: 10f)
        };
    }
}

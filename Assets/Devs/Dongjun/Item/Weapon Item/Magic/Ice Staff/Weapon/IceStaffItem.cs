using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStaffItem : MagicBase
{
    // Timer
    public readonly TimerStat CD_Main_Shoot = new TimerStat();

    public ProjectileData BulletData
    { get; private set; }

    public FloatStat ManaUsage_Main_Shoot;

    private void Start()
    {
        CD_Main_Shoot.SetTick(gameObject).ToEnd();
    }
    public override void InitStats()
    {
        CD_Main_Shoot.EndTime = new FloatStat(1f, min: 0.01f);

        AttackData = new AttackData(15);

        BulletData = new ProjectileData()
        {
            moveSpeed = new FloatStat(25f, min: 0f),
            travelDist = new FloatStat(0f, min: 0f, max: 10f)
        };

        ManaUsage_Main_Shoot = new FloatStat(5, min: 0);
    }
    public override void ResetStats()
    {
        CD_Main_Shoot.EndTime.ResetMinMax();

        AttackData.Reset();

        BulletData.Reset();
        ManaUsage_Main_Shoot.Reset();
    }
}

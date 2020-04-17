using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStaffItem : MagicBase
{
    // Timer
    public readonly TimerStat Main_Cooldown = new TimerStat();

    // Main
    public FloatStat Main_ManaUsage;
    public ProjectileData Main_BulletData
    { get; private set; }

    // Sub
    public FloatStat Sub_Damage;
    public FloatStat Sub_ManaUsagePerSec;
    public TimerStat Sub_CastDur = new TimerStat();
    public TimerStat Sub_ManaUsageTick = new TimerStat();
    public TimerStat Sub_DamageTick = new TimerStat();

    // Special
    public IntStat Spec_FrozenSoulUsage;
    public TimerStat Spec_CastDur = new TimerStat();
    public TimerStat Spec_IceBlockDur = new TimerStat();

    private void Start()
    {
        // Main
        Main_Cooldown.SetTick(gameObject).ToEnd();

        // Sub
        Sub_CastDur.SetTick(gameObject).SetActive(false);
        Sub_DamageTick.SetTick(gameObject).SetActive(false);
        Sub_ManaUsageTick.SetTick(gameObject).SetActive(false);

        // Special
        Spec_CastDur.SetTick(gameObject).SetActive(false);
        Spec_IceBlockDur.SetTick(gameObject).SetActive(false);
    }
    public override void InitStats()
    {
        // Main
        AttackData = new AttackData(15);
        Main_Cooldown.EndTime = new FloatStat(1f, min: 0.01f);
        Main_ManaUsage = new FloatStat(5, min: 0);
        Main_BulletData = new ProjectileData()
        {
            moveSpeed = new FloatStat(25f, min: 0f),
            travelDist = new FloatStat(0f, min: 0f, max: 10f)
        };

        // Sub

        // Special
        Spec_CastDur.EndTime = new FloatStat(0.15f);
    }
    public override void ResetStats()
    {
        // Main
        AttackData.Reset();
        Main_Cooldown.EndTime.ResetMinMax();
        Main_ManaUsage.Reset();
        Main_BulletData.Reset();

        // Sub
        Sub_Damage.Reset();
        Sub_DamageTick.EndTime.ResetMinMax();
        Sub_ManaUsagePerSec.Reset();

        // Sepcial
        Spec_CastDur.EndTime.ResetMinMax();
    }
}

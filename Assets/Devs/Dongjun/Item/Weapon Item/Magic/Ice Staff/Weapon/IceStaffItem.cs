using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStaffItem : MagicBase
{
    // Main
    public readonly TimerStat Main_CD = new TimerStat();
    public FloatStat Main_ManaUsage;
    public ProjectileData Main_IceBoltData
    { get; private set; }

    // Sub
    public TimerStat Sub_CastTime = new TimerStat();
    public TimerStat Sub_DamageTick = new TimerStat();
    public AttackData Sub_DamagePerTick;
    public TimerStat Sub_ManaUsageTick = new TimerStat();
    public FloatStat Sub_ManaUsagePerTick;

    // Special
    public TimerStat Spec_CastTime = new TimerStat();
    public IntStat Spec_FrozenSoulUsage;
    public TimerStat Spec_IceBlockDur = new TimerStat();

    private void Start()
    {
        // Main Timer
        Main_CD.SetTick(gameObject).ToEnd();

        // Sub Timer
        Sub_CastTime.SetTick(gameObject).SetActive(false);
        Sub_DamageTick.SetTick(gameObject).SetActive(false).ToEnd();
        Sub_ManaUsageTick.SetTick(gameObject).SetActive(false).ToEnd();

        // Special Timer
        Spec_CastTime.SetTick(gameObject).SetActive(false);
    }
    public override void InitStats()
    {
        AttackData = new AttackData(15);

        // Main: Cooldown
        Main_CD.EndTime = new FloatStat(0.7f, min: 0.01f);
        // Main: Mana Usage
        Main_ManaUsage = new FloatStat(4f, min: 0);
        // Main: Ice Bolt
        Main_IceBoltData = new ProjectileData()
        {
            moveSpeed = new FloatStat(25f, min: 0f),
            travelDist = new FloatStat(0f, min: 0f, max: 10f)
        };

        // Sub: Cast Time
        Sub_CastTime.EndTime = new FloatStat(0.2f, min: 0f);
        // Sub: Damage
        Sub_DamageTick.EndTime = new FloatStat(0.5f, min: 0f);
        Sub_DamagePerTick = new AttackData(2f, gameObject);
        // Sub: Mana Usage
        Sub_ManaUsageTick.EndTime = new FloatStat(0.25f, min: 0f);
        Sub_ManaUsagePerTick = new FloatStat(0.5f, min: 0f);

        // Special: Cast Time
        Spec_CastTime.EndTime = new FloatStat(0.15f, min: 0f);
        // Special: Frozen Soul Usage
        Spec_FrozenSoulUsage = new IntStat(5, min: 0);
        // Special: Ice Block
        Spec_IceBlockDur.EndTime = new FloatStat(5f, min: 0f);
    }
    public override void ResetStats()
    {
        AttackData.Reset();

        // Main
        Main_CD.EndTime.ResetMinMax();
        Main_ManaUsage.Reset();
        Main_IceBoltData.Reset();

        // Sub
        Sub_CastTime.EndTime.ResetMinMax();
        Sub_DamageTick.EndTime.ResetMinMax();
        Sub_DamagePerTick.Reset();
        Sub_ManaUsageTick.EndTime.ResetMinMax();
        Sub_ManaUsagePerTick.Reset();

        // Sepcial
        Spec_CastTime.EndTime.ResetMinMax();
        Spec_FrozenSoulUsage.Reset();
        Spec_IceBlockDur.EndTime.ResetMinMax();
    }
}

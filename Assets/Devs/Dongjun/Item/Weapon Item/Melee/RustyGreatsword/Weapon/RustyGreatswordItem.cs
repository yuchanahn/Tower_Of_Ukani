﻿using UnityEngine;

public class RustyGreatswordItem : MeleeItem
{
    public readonly TimerStat Cooldown_Basic = new TimerStat();

    public float heavyChargePerSec { get; private set; } = 3f;
    public float HeavyFullChargeTime { get; private set; } = 3f;
    public float HeavyChargeTime = 0;

    public FloatStat HeavyAttack_StaminaUsage;
    public FloatStat SlamAttack_StaminaUsage;

    protected override void Start()
    {
        base.Start();
        Cooldown_Basic.SetTick(gameObject).ToEnd();
    }
    public override void InitStats()
    {
        // Attack Data
        AttackData = new AttackData(5);
        AttackData_Heavy = new AttackData(20);
        AttackData_Slam = new AttackData(10);
        AttackData_Dash = new AttackData(10);

        // Timmer Data
        Dur_Basic.EndTime = new FloatStat(1f, min: 0.01f);
        Cooldown_Basic.EndTime = new FloatStat(0.5f, min: 0.01f);
        Dur_Heavy.EndTime = new FloatStat(0.6f, min: 0.01f);
        Dur_Slam.EndTime = new FloatStat(0.35f, min: 0.01f);

        // Stamina Usage
        HeavyAttack_StaminaUsage = new FloatStat(1f, min: 0f);
        SlamAttack_StaminaUsage = new FloatStat(1f, min: 0f);
    }
}
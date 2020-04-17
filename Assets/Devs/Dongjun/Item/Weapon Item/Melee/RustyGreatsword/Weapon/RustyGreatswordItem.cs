using UnityEngine;

public class RustyGreatswordItem : MeleeItem
{
    public readonly TimerStat Basic_CD = new TimerStat();

    public float Heavy_FullChargeTime
    { get; private set; }
    public float Heavy_CurChargeTime = 0f;

    public FloatStat Heavy_StaminaUsage;
    public FloatStat Slam_StaminaUsage;

    protected override void Start()
    {
        base.Start();
        Basic_CD.SetTick(gameObject).ToEnd();
    }
    public override void InitStats()
    {
        AttackData = new AttackData(10);

        // Basic
        Basic_Dur.EndTime = new FloatStat(1f, min: 0.01f);
        Basic_CD.EndTime = new FloatStat(0.5f, min: 0.01f);

        // Heavy
        Heavy_AttackData = new AttackData(30);
        Heavy_StaminaUsage = new FloatStat(1f, min: 0f);
        Heavy_FullChargeTime = 3f;
        Heavy_Dur.EndTime = new FloatStat(0.6f, min: 0.01f);

        // Slam
        Slam_AttackData = new AttackData(20);
        Slam_StaminaUsage = new FloatStat(1f, min: 0f);
        Slam_Dur.EndTime = new FloatStat(0.35f, min: 0.01f);

        // Dash
        Dash_AttackData = new AttackData(10);
    }
    public override void ResetStats()
    {
        base.ResetStats();
        Basic_CD.EndTime.ResetMinMax();
    }
}

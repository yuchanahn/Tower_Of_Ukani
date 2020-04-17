using UnityEngine;

public class FistItem : MeleeItem
{
    public float Heavy_FullChargeTime
    { get; private set; }
    public float Heavy_CurChargeTime = 0f;

    public override void InitStats()
    {
        AttackData = new AttackData(5);
        
        // Basic
        Basic_Dur.EndTime = new FloatStat(0.35f, min: 0.01f);

        // Heavy
        Heavy_AttackData = new AttackData(20);
        Heavy_FullChargeTime = 2f;
        Heavy_Dur.EndTime = new FloatStat(0.5f, min: 0.01f);

        // Slam
        Slam_AttackData = new AttackData(15);
        Slam_Dur.EndTime = new FloatStat(0.25f, min: 0.01f);

        // Dash
        Dash_AttackData = new AttackData(10);
    }
}

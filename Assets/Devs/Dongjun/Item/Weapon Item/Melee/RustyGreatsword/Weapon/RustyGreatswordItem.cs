using UnityEngine;

public class RustyGreatswordItem : MeleeItem
{
    public float heavyChargePerSec { get; private set; } = 3f;
    public float HeavyFullChargeTime { get; private set; } = 4f;
    public float HeavyChargeTime = 0;

    public override void InitStats()
    {
        // Attack Data
        AttackData = new AttackData(5);
        AttackData_Heavy = new AttackData(20);
        AttackData_Slam = new AttackData(4);
        AttackData_Dash = new AttackData(5);

        // Timmer Data
        Dur_Basic.EndTime = new FloatStat(1.5f, min: 0.01f);
        Dur_Heavy.EndTime = new FloatStat(0.5f, min: 0.01f);
        Dur_Slam.EndTime = new FloatStat(0.25f, min: 0.01f);
    }
}

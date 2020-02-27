using UnityEngine;

public class FistItem : MeleeItem
{
    public float heavyChargePerSec { get; private set; } = 3f;
    public float HeavyFullChargeTime { get; private set; } = 4f;
    public float HeavyChargeTime = 0;

    public override void InitStats()
    {
        // Attack Data
        AttackData = new AttackData(2);
        AttackData_Heavy = new AttackData(15);
        AttackData_Slam = new AttackData(4);
        AttackData_Dash = new AttackData(5);

        // Timmer Data
        Dur_Basic.EndTime = new FloatStat(0.35f, min: 0.01f);
        Dur_Heavy.EndTime = new FloatStat(0.5f, min: 0.01f);
        Dur_Slam.EndTime = new FloatStat(0.25f, min: 0.01f);
    }
}

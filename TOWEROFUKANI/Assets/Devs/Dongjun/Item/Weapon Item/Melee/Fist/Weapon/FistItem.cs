using UnityEngine;

public class FistItem : MeleeItem
{
    public float heavyChargePerSec = 3f;

    public override void InitStats()
    {
        // Attack Data
        AttackData = new AttackData(2);
        attackData_Heavy = new AttackData()
        {
            damage = new FloatStat(1, min: 0, max: 20)
        };
        attackData_Slam = new AttackData(4);
        attackData_Dash = new AttackData(5);

        // Timmer Data
        basicAttackTimer.EndTime = new FloatStat(0.2f, min: 0.01f);
    }
}

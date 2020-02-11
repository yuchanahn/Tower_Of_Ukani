using UnityEngine;

public class FistItem : MeleeItem
{
    public override void InitStats()
    {
        // Attack Data
        attackData = new AttackData(5);

        // Timmer Data
        basicAttackTimer.EndTime = new FloatStat(0.2f, min: 0.01f);
    }
}

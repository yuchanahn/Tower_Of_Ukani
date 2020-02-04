using UnityEngine;

public class WoodenShortBowItem : BowItem
{
    public override void InitStats()
    {
        // Attack Data
        attackData = new AttackData(5);

        // Timer Data
        shootTimer.EndTime = new FloatStat(0.2f, min: 0f);
        drawTimer.EndTime = new FloatStat(0.5f, min: 0.01f);

        // Arrow Data
        arrowData = new ProjectileData()
        {
            moveSpeed = new FloatStat(30f, min: 0f),
            travelDist = new FloatStat(min: 0f, max: 30f),
            gravity = new FloatStat(40f, min: 0f)
        };
    }
}

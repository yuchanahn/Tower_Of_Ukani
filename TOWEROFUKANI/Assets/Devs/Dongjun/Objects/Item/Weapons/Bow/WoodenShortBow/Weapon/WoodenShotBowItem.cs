using UnityEngine;

public class WoodenShotBowItem : BowItem
{
    protected override void Awake()
    {
        base.Awake();

        // Timer Data
        shootTimer.StartAsEnded = true;
        shootTimer.EndTime = new FloatStat(0.2f, min: 0f);
        drawTimer.EndTime = new FloatStat(0.5f, min: 0.01f);

        // Arrow Data
        arrowData.damage = new IntStat(5, min: 0);
        arrowData.moveSpeed = new FloatStat(30f, min: 0f);
        arrowData.gravity = new FloatStat(1f, min: 0f);
        arrowData.maxTravelDist = new FloatStat(30f, min: 0f);
    }
}

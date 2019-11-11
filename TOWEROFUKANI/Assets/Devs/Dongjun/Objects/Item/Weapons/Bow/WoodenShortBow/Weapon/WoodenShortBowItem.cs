using UnityEngine;

public class WoodenShortBowItem : BowItem
{
    public override void InitStats()
    {
        // Timer Data
        shootTimer.StartAsEnded = true;
        shootTimer.EndTime = new FloatStat(0.2f, min: 0f);
        drawTimer.EndTime = new FloatStat(0.5f, min: 0.01f);

        // Arrow Data
        arrowData = new WeaponProjectileData()
        {
            attackData = new AttackData(5),
            moveSpeed = new FloatStat(30f, min: 0f),
            gravity = new FloatStat(1f, min: 0f),
            maxTravelDist = new FloatStat(30f, min: 0f)
        };
    }
}

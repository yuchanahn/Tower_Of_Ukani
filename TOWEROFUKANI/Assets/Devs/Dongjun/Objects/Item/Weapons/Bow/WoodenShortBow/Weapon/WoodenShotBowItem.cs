using UnityEngine;

public class WoodenShotBowItem : BowItem
{
    protected override void Awake()
    {
        base.Awake();

        shootTimer.StartAsEnded = true;
        shootTimer.EndTime = new FloatStat(0.2f, min: 0f);

        drawTimer.EndTime = new FloatStat(0.5f, min: 0.01f);
    }
}

using UnityEngine;

public class WoodenShortbowItem : BowItem
{
    public override void InitStats()
    {
        AttackData = new AttackData(25);

        // Main
        Main_Shoot_CD.EndTime = new FloatStat(0.2f, min: 0f);
        Main_Draw_Dur.EndTime = new FloatStat(0.5f, min: 0.01f);
        Main_ArrowData = new ProjectileData()
        {
            moveSpeed = new FloatStat(30f, min: 0f),
            travelDist = new FloatStat(min: 0f, max: 30f),
            gravity = new FloatStat(40f, min: 0f)
        };
    }
}

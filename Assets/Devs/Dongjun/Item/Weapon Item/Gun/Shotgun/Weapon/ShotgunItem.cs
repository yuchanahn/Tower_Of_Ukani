using System.Collections;
using UnityEngine;

public class ShotgunItem : GunItem
{
    public int PelletCount { get; private set; } = 4;
    public float PelletAngle { get; private set; } = 10f;

    public override void InitStats()
    {
        AttackData = new AttackData(15);

        // Main
        Main_Shoot_CD.EndTime = new FloatStat(0.1f, min: 0.01f);
        Main_Reload_Dur.EndTime = new FloatStat(0.12f, min: 0.01f);
        Main_SwapMagazine_Dur.EndTime = new FloatStat(1f, min: 0.01f);
        Main_MagazineSize = new IntStat(2, min: 0);
        Main_BulletData = new ProjectileData()
        {
            moveSpeed = new FloatStat(45f, min: 0f),
            travelDist = new FloatStat(0f, min: 0f, max: 7f)
        };
    }
}

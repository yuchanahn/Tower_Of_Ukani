using System.Collections;
using UnityEngine;

public class ShotgunItem : GunItem
{
    public int PelletCount { get; private set; } = 4;
    public float PelletAngle { get; private set; } = 10f;

    public override void InitStats()
    {
        // Attack Data
        AttackData = new AttackData(8);

        // Ammo Data
        MagazineSize = new IntStat(2, min: 0);

        // Bullet Data
        BulletData = new ProjectileData()
        {
            moveSpeed = new FloatStat(45f, min: 0f),
            travelDist = new FloatStat(0f, min: 0f, max: 7f)
        };

        // Timer Data
        Timer_Shoot.EndTime = new FloatStat(0.1f, min: 0.01f);
        Timer_Reload.EndTime = new FloatStat(0.12f, min: 0.01f);
        Timer_SwapMagazine.EndTime = new FloatStat(1f, min: 0.01f);
    }
}

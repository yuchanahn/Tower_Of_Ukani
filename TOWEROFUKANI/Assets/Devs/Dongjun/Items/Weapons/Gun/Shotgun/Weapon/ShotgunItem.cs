using System.Collections;
using UnityEngine;

public class ShotgunItem : GunItem
{
    public override void InitStats()
    {
        // Attack Data
        attackData = new AttackData(8);

        // Timer Data
        shootTimer.EndTime = new FloatStat(0.1f, min: 0.01f);
        reloadTimer.EndTime = new FloatStat(0.12f, min: 0.01f);
        swapMagazineTimer.EndTime = new FloatStat(1f, min: 0.01f);

        // Bullet Data
        bulletData = new ProjectileData()
        {
            moveSpeed = new FloatStat(45f, min: 0f),
            travelDist = new FloatStat(0f, min: 0f, max: 7f)
        };

        // Ammo Data
        magazineSize = new IntStat(2, min: 0);
    }
}

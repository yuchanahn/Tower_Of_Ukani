using System.Collections;
using UnityEngine;

public class ShotgunItem : GunItem
{
    public override void InitStats()
    {
        // Timer Data
        shootTimer.StartAsEnded = true;
        shootTimer.EndTime = new FloatStat(0.1f, min: 0.01f);
        reloadTimer.EndTime = new FloatStat(0.12f, min: 0.01f);
        swapMagazineTimer.EndTime = new FloatStat(1f, min: 0.01f);

        // Bullet Data
        bulletData = new WeaponProjectileData()
        {
            attackData = new AttackData(8),
            moveSpeed = new FloatStat(45f, min: 0f),
            maxTravelDist = new FloatStat(7f, min: 0f),
        };

        // Ammo Data
        magazineSize = new IntStat(2, min: 0);
    }
}

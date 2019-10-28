using System.Collections;
using UnityEngine;

public class ShotgunItem : GunItem
{
    protected override void Awake()
    {
        base.Awake();

        // Timer Data
        shootTimer.StartAsEnded = true;
        shootTimer.EndTime = new FloatStat(0.1f, min: 0.01f);
        reloadTimer.EndTime = new FloatStat(0.12f, min: 0.01f);
        swapMagazineTimer.EndTime = new FloatStat(1f, min: 0.01f);

        // Bullet Data
        bulletData.damage = new IntStat(1, min: 0);
        bulletData.moveSpeed = new FloatStat(45f, min: 0f);
        bulletData.maxTravelDist = new FloatStat(7f, min: 0f);

        // Ammo Data
        magazineSize = new IntStat(2, min: 0);
    }
}

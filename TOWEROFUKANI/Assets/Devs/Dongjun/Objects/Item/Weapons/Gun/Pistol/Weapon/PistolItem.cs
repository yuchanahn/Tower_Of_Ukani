using UnityEngine;

public class PistolItem : GunItem
{
    protected override void Awake()
    {
        base.Awake();

        // Timer Data
        shootTimer.StartAsEnded = true;
        shootTimer.EndTime = new FloatStat(0.15f, min: 0.01f);
        reloadTimer.EndTime = new FloatStat(0.5f, min: 0.01f);
        swapMagazineTimer.EndTime = new FloatStat(0.8f, min: 0.01f);

        // Bullet Data
        bulletData.damage = new IntStat(1, min: 0);
        bulletData.moveSpeed = new FloatStat(45f, min: 0f);
        bulletData.maxTravelDist = new FloatStat(10f, min: 0f);

        // Ammo Data
        magazineSize = new IntStat(6, min: 0);
    }
}

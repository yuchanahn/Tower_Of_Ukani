using UnityEngine;

public class PistolItem : GunItem
{
    public override void InitStats()
    {
        // Attack Data
        attackData = new AttackData(1);

        // Timer Data
        shootTimer.EndTime = new FloatStat(0.15f, min: 0.01f);
        reloadTimer.EndTime = new FloatStat(0.5f, min: 0.01f);
        swapMagazineTimer.EndTime = new FloatStat(0.8f, min: 0.01f);

        // Bullet Data
        bulletData = new ProjectileData()
        {
            moveSpeed = new FloatStat(45f, min: 0f),
            travelDist = new FloatStat(0f, min: 0f, max: 10f)
        };

        // Ammo Data
        magazineSize = new IntStat(6, min: 0);

        // Upgrade
        switch (ItemLevel)
        {
            case 1:
                break;
            case 2:
                attackData = new AttackData(2);
                break;
            default:
                attackData = new AttackData(3);
                break;
        }
    }
}

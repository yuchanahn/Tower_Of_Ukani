using UnityEngine;

public class PistolItem : GunItem
{
    public override void InitStats()
    {
        // Timer Data
        shootTimer.StartAsEnded = true;
        shootTimer.EndTime = new FloatStat(0.15f, min: 0.01f);
        reloadTimer.EndTime = new FloatStat(0.5f, min: 0.01f);
        swapMagazineTimer.EndTime = new FloatStat(0.8f, min: 0.01f);

        // Bullet Data
        bulletData = new WeaponProjectileData()
        {
            attackData = new AttackData(1),
            moveSpeed = new FloatStat(45f, min: 0f),
            maxTravelDist = new FloatStat(10f, min: 0f),
        };

        // Ammo Data
        magazineSize = new IntStat(6, min: 0);

        // Upgrade Per Count
        switch (Count)
        {
            case 1:
                break;
            case 2:
                bulletData.attackData = new AttackData(2);
                break;
            default:
                bulletData.attackData = new AttackData(3);
                break;
        }
    }
}

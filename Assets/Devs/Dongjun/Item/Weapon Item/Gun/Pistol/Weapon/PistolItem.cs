using UnityEngine;

public class PistolItem : GunItem
{
    public override void InitStats()
    {
        // Attack Data
        AttackData = new AttackData(8);

        // Ammo Data
        MagazineSize = new IntStat(6, min: 0);

        // Bullet Data
        BulletData = new ProjectileData()
        {
            moveSpeed = new FloatStat(45f, min: 0f),
            travelDist = new FloatStat(0f, min: 0f, max: 10f)
        };

        // Timer Data
        CD_Main_Shoot.EndTime = new FloatStat(0.15f, min: 0.01f);
        Dur_Main_Reload.EndTime = new FloatStat(0.5f, min: 0.01f);
        Dur_Main_SwapMagazine.EndTime = new FloatStat(0.8f, min: 0.01f);

        // Upgrade
        switch (ItemLevel)
        {
            case 1: break;

            case 2:
                AttackData = new AttackData(10);
                break;

            default:
                AttackData = new AttackData(12);
                break;
        }
    }
}

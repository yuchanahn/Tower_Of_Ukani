using UnityEngine;

public class PistolItem : GunItem
{
    public override void InitStats()
    {
        AttackData = new AttackData(8);

        // Main
        Main_Shoot_CD.EndTime = new FloatStat(0.15f, min: 0.01f);
        Main_Reload_Dur.EndTime = new FloatStat(0.5f, min: 0.01f);
        Main_SwapMagazine_Dur.EndTime = new FloatStat(0.8f, min: 0.01f);
        Main_MagazineSize = new IntStat(6, min: 0);
        Main_BulletData = new ProjectileData()
        {
            moveSpeed = new FloatStat(45f, min: 0f),
            travelDist = new FloatStat(0f, min: 0f, max: 10f)
        };

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

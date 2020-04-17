using UnityEngine;

public class MachineGunItem : GunItem
{
    #region Var: Inspector
    [Header("Ammo Belt")]
    public Transform ammoBelt;
    public float ammoBeltAmmoCount;
    #endregion

    #region Prop: 
    public float AmmoBeltMaxY => 0.0625f * ammoBeltAmmoCount;
    #endregion

    public override void InitStats()
    {
        AttackData = new AttackData(5);

        // Main
        Main_Shoot_CD.EndTime = new FloatStat(0.15f, min: 0.01f);
        Main_Reload_Dur.EndTime = new FloatStat(0.5f, min: 0.01f);
        Main_SwapMagazine_Dur.EndTime = new FloatStat(1f, min: 0.01f);
        Main_MagazineSize = new IntStat(20, min: 0);
        Main_BulletData = new ProjectileData()
        {
            moveSpeed = new FloatStat(45f, min: 0f),
            travelDist = new FloatStat(min: 0f, max: 10f)
        };

        // Upgrade
        switch (ItemLevel)
        {
            case 1: break;

            case 2:
                Main_Shoot_CD.EndTime.Base = 0.1f;
                break;

            default:
                Main_Shoot_CD.EndTime.Base = 0.05f;
                break;
        }
    }

    public void UpdateAmmoBeltPos()
    {
        ammoBelt.localPosition =
            new Vector2(0, Mathf.Lerp(0, AmmoBeltMaxY, 1 - ((float)LoadedBullets / Main_MagazineSize.Value)));
    }
    public void HideAmmoBelt()
    {
        ammoBelt.localPosition = new Vector2(0, AmmoBeltMaxY);
    }
}

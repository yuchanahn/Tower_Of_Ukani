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
        // Attack Data
        AttackData = new AttackData(1);

        // Ammo Data
        MagazineSize = new IntStat(20, min: 0);

        // Bullet Data
        BulletData = new ProjectileData()
        {
            moveSpeed = new FloatStat(45f, min: 0f),
            travelDist = new FloatStat(min: 0f, max: 10f)
        };

        // Timmer Data
        Timer_Shoot.EndTime = new FloatStat(0.15f, min: 0.01f);
        Timer_Reload.EndTime = new FloatStat(0.5f, min: 0.01f);
        Timer_SwapMagazine.EndTime = new FloatStat(1f, min: 0.01f);

        // Upgrade
        switch (ItemLevel)
        {
            case 1:
                break;
            case 2:
                Timer_Shoot.EndTime.Base = 0.1f;
                break;
            default:
                Timer_Shoot.EndTime.Base = 0.05f;
                break;
        }
    }

    public void UpdateAmmoBeltPos()
    {
        ammoBelt.localPosition =
            new Vector2(0, Mathf.Lerp(0, AmmoBeltMaxY, 1 - ((float)LoadedBullets / MagazineSize.Value)));
    }
    public void HideAmmoBelt()
    {
        ammoBelt.localPosition = new Vector2(0, AmmoBeltMaxY);
    }
}

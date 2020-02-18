using UnityEngine;

public class RoseOfLove : ActiveItem
{
    [SerializeField] private WeaponProjectile roseProjectile;

    private ProjectileData roseProjectileData;

    public override void InitStats()
    {
        cooldownTimer.EndTime = 7f;

        roseProjectileData = new ProjectileData()
        {
            moveSpeed = new FloatStat(2f, min: 0),
            travelDist = new FloatStat(20, min: 0),
            gravity = new FloatStat(30f, min: 0)
        };
    }

    protected override void OnActivate()
    {
        // Restart Cooldown Timer
        cooldownTimer.Reset();

        // Spawn Rose
        WeaponProjectile rose = roseProjectile.Spawn(GM.PlayerPos, Quaternion.identity);
        rose.InitData(Global.Inst.MainCam.ScreenToWorldPoint(Input.mousePosition) - GM.PlayerPos, roseProjectileData);

        // Deactivate Item
        Deactivate();
    }
}

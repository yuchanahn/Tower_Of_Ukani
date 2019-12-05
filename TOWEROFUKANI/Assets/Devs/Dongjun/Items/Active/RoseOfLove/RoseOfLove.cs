using UnityEngine;

public class RoseOfLove : ActiveItem
{
    [SerializeField] private WeaponProjectile roseProjectile;

    private ProjectileData roseProjectileData;

    private void Start()
    {
        cooldownTimer.EndTime = 7f;

        roseProjectileData = new ProjectileData()
        {
            moveSpeed = new FloatStat(8, min: 0),
            travelDist = new FloatStat(20, min: 0),
            gravity = new FloatStat(0.3f, min: 0)
        };
    }

    public override void Activate()
    {
        // Restart Cooldown Timer
        cooldownTimer.Restart();

        // Spawn Rose
        WeaponProjectile rose = roseProjectile.Spawn(GM.PlayerPos, Quaternion.identity);
        rose.transform.right = (Vector2)(Global.Inst.MainCam.ScreenToWorldPoint(Input.mousePosition) - GM.PlayerPos);
        rose.InitData(roseProjectileData);
    }
}

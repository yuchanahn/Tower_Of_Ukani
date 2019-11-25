using UnityEngine;

public class RoseOfLove : ActiveItem
{
    [SerializeField] private WeaponProjectile roseProjectile;

    private WeaponProjectileData roseProjectileData;

    private void Start()
    {
        cooldownTimer.EndTime = 0.5f;

        roseProjectileData.moveSpeed = new FloatStat(8, min: 0);
        roseProjectileData.gravity = new FloatStat(0.3f, min: 0);
        roseProjectileData.maxTravelDist = new FloatStat(20, min: 0);
    }

    public override void Activate()
    {
        if (!cooldownTimer.IsEnded)
            return;

        cooldownTimer.Restart();

        WeaponProjectile rose = roseProjectile.Spawn(GM.PlayerPos, Quaternion.identity);
        rose.transform.right = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - GM.PlayerPos);
        rose.SetData(roseProjectileData);
    }
}

using UnityEngine;

public class PhantomPistol : PassiveItem
{
    [SerializeField] private PhantomPistolObject phantomPistolObjectPrefab;

    private ItemEffect onShoot;
    private PhantomPistolObject phantomPistolObject;

    private AttackData attackData;
    private ProjectileData projectileData;

    #region Method: Initialize
    public override void InitStats()
    {
        attackData = new AttackData(3);

        projectileData = new ProjectileData()
        {
            moveSpeed = new FloatStat(40f, min: 0f),
            travelDist = new FloatStat(0f, min: 0f, max: 6f)
        };
    }
    #endregion

    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        onShoot = new ItemEffect(GetType(), OnShoot);
        ItemEffectManager.AddEffect(PlayerActions.GunShoot, onShoot);

        // Spawn Phantom Pistol Object
        phantomPistolObject = Instantiate(phantomPistolObjectPrefab, GM.PlayerObj.transform.position, Quaternion.identity);
    }
    public override void OnDrop()
    {
        base.OnDrop();
        ItemEffectManager.RemoveEffect(PlayerActions.GunShoot, onShoot);

        // Destroy Phantom Pistol Object
        Destroy(phantomPistolObject);
    }

    protected override void SetBonusStats(WeaponItem weapon)
    {

    }

    private void OnShoot()
    {
        Bullet bullet = phantomPistolObject.SpawnBullet();
        bullet.InitData(bullet.transform.right, projectileData, attackData);
    }
}

using UnityEngine;

public class PhantomPistol : PassiveItem
{
    [SerializeField] private PhantomPistolObject phantomPistolObjectPrefab;

    private ActionEffect onShoot;
    private PhantomPistolObject phantomPistolObject;

    private AttackData attackData;
    private ProjectileData projectileData;

    #region Method: Stats
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

    #region Method: Item
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        onShoot = this.CreateActionEffect(OnShoot);

        ActionEffectManager.AddEffect(PlayerActions.GunShoot, onShoot);

        // Spawn Phantom Pistol Object
        phantomPistolObject = Instantiate(phantomPistolObjectPrefab, GM.PlayerObj.transform.position, Quaternion.identity);
    }
    protected override void OnRemovedFromInventory()
    {
        ActionEffectManager.RemoveEffect(PlayerActions.GunShoot, onShoot);

        // Destroy Phantom Pistol Object
        Destroy(phantomPistolObject);
    }
    #endregion

    #region Method: Item Effect
    private void OnShoot()
    {
        Bullet bullet = phantomPistolObject.SpawnBullet();
        bullet.InitData(bullet.transform.right, projectileData, attackData);
    }
    #endregion
}

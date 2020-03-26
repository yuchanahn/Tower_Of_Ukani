using UnityEngine;

public class PhantomPistol : PassiveItem
{
    #region Var: Insepctor
    [SerializeField] private PhantomPistolObject phantomPistolObjectPrefab;
    #endregion

    #region Var: Phantom Pistol Object
    private PhantomPistolObject phantomPistolObject;
    #endregion

    #region Var: Item Effect
    private PlayerActionEvent onShoot;
    #endregion

    #region Var: Item Stats
    private AttackData attackData;
    private ProjectileData projectileData;
    #endregion

    #region Method: Item
    public override void InitStats()
    {
        attackData = new AttackData(3);

        projectileData = new ProjectileData()
        {
            moveSpeed = new FloatStat(40f, min: 0f),
            travelDist = new FloatStat(0f, min: 0f, max: 6f)
        };
    }
    protected override void InitEvents()
    {
        onShoot = this.NewPlayerActionEvent(() =>
        {
            Bullet bullet = phantomPistolObject.SpawnBullet();
            bullet.InitData(bullet.transform.right, projectileData, attackData);
        });
    }

    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        PlayerActionEventManager.AddEvent(PlayerActions.GunShoot, onShoot);

        // Spawn Phantom Pistol Object
        phantomPistolObject = Instantiate(phantomPistolObjectPrefab, GM.PlayerObj.transform.position, Quaternion.identity);
    }
    protected override void OnRemovedFromInventory()
    {
        PlayerActionEventManager.RemoveEvent(PlayerActions.GunShoot, onShoot);

        // Destroy Phantom Pistol Object
        Destroy(phantomPistolObject);
    }
    #endregion
}

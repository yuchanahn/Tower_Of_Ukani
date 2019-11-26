using UnityEngine;

public class PhantomPistol : PassiveItem
{
    [SerializeField] private PhantomPistolObject phantomPistolObjectPrefab;

    private ItemEffect onShoot;
    private PhantomPistolObject phantomPistolObject;

    private AttackData attackData;
    private ProjectileData projectileData;

    private void Start()
    {
        attackData = new AttackData(3);

        projectileData = new ProjectileData()
        {
            moveSpeed = new FloatStat(40f, min: 0f),
            travelDist = new FloatStat(0f, min: 0f, max: 6f)
        };
    }

    public override void OnAdd()
    {
        onShoot = new ItemEffect(GetType(), OnShoot);
        ItemEffectManager.AddEffect(PlayerActions.GunShoot, onShoot);

        // Spawn Phantom Pistol Object
        phantomPistolObject = Instantiate(phantomPistolObjectPrefab, GM.PlayerObj.transform.position, Quaternion.identity);
    }
    public override void OnRemove()
    {
        base.OnRemove();
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
        bullet.InitData(projectileData, attackData);
    }
}

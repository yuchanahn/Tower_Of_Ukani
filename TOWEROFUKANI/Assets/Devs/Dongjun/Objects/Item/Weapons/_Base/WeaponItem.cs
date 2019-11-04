using System.Collections.Generic;
using UnityEngine;

#region Weapon Base
public abstract class WeaponController_Base : CLA_Main { }
public abstract class WeaponController<TItem> : WeaponController_Base
    where TItem : WeaponItem
{
    protected TItem weaponItem;
    protected override void Awake()
    {
        base.Awake();
        weaponItem = GetComponent<WeaponItem>() as TItem;
    }
}
public abstract class WeaponItem : Item 
{
    public enum WeaponTag
    {
        Gun,
        Bow,
    }

    #region Var: Inspector
    [SerializeField] private GameObject spriteRoot;
    [SerializeField] private float pivotPointY;
    [SerializeField] private WeaponTag[] weaponTags;
    #endregion

    #region Var: Properties
    public GameObject SpriteRoot => spriteRoot;
    public float PivotPointY => pivotPointY;
    public HashSet<WeaponTag> WeaponTags
    { get; private set; } = new HashSet<WeaponTag>();
    public bool IsSelected
    { get; protected set; } = false;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        if (weaponTags is null)
            return;

        for (int i = 0; i < weaponTags.Length; i++)
            WeaponTags.Add(weaponTags[i]);
    }
    #endregion

    #region Method: Item
    public override void OnAdd()
    {
        Select(Inventory.WeaponHotbar.GetSelected() == this);
    }
    public override void OnRemove()
    {
        Select(false);

        // Deactivate Weapon
        gameObject.SetActive(false);
        transform.SetParent(null);

        // Activate Dropped Item
        DroppedItem.Activate();
    }
    public void Select(bool select)
    {
        IsSelected = select;
        spriteRoot.SetActive(select);
    }
    #endregion
}
#endregion

#region Gun
public abstract class GunController<TGun> : WeaponController<TGun> where TGun : GunItem { }
public abstract class GunItem : WeaponItem
{
    // Timer Data
    public TimerStat shootTimer;
    public TimerStat reloadTimer;
    public TimerStat swapMagazineTimer;

    // Bullet Data
    public WeaponProjectileData bulletData;

    // Ammo Data
    public IntStat magazineSize;
    public int loadedBullets;
    public bool isBulletLoaded;

    protected void Start()
    {
        // Init Timer
        shootTimer.Init(gameObject);
        reloadTimer.Init(gameObject);
        swapMagazineTimer.Init(gameObject);

        // Init Ammo
        loadedBullets = magazineSize.Value;
    }
}
#endregion

#region Bow
public abstract class BowController<TBow> : WeaponController<TBow> where TBow : BowItem { }
public abstract class BowItem : WeaponItem
{
    // Visual
    [SerializeField] protected GameObject arrowVisual;

    // Timer
    public TimerStat shootTimer;
    public TimerStat drawTimer;

    // Arrow Data
    public WeaponProjectileData arrowData;

    // Extra Info
    [HideInInspector] public bool hasBeenDrawn = false;
    [HideInInspector] public float drawPower = 0;

    public GameObject ArrowVisual => arrowVisual;

    protected void Start()
    {
        shootTimer.Init(gameObject);
        drawTimer.Init(gameObject);
    }
}
#endregion
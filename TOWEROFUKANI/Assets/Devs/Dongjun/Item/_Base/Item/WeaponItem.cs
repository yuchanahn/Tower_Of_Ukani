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

    protected override void RunConditionLogic()
    {
        if (!weaponItem.IsSelected)
        {
            ChangeAction(DefaultAction);
            return;
        }

        base.RunConditionLogic();
    }
}

public abstract class WeaponItem : UpgradableItem 
{
    public enum WeaponType
    {
        None,
        All,
        Gun,
        Bow,
    }

    #region Var: Inspector
    [Header("Weapon Info")]
    [SerializeField] private GameObject spriteRoot;
    [SerializeField] private float pivotPointY;
    [SerializeField] private WeaponType[] weaponTags;
    #endregion

    #region Var: Properties
    public GameObject SpriteRoot => spriteRoot;
    public float PivotPointY => pivotPointY;
    public WeaponType[] WeaponTags => weaponTags;
    public bool IsSelected
    { get; protected set; } = false;
    #endregion

    #region Var: Stats
    public AttackData attackData;
    #endregion

    #region Method Override: Add / Move / Drop
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        gameObject.SetActive(true);
        transform.SetParent(GM.PlayerObj.transform);
        transform.localPosition = new Vector2(0, PivotPointY);

        Select(Inventory is PlayerWeaponHotbar && (Inventory as PlayerWeaponHotbar).CurWeapon == this);
    }
    public override void OnMove()
    {
        Select(Inventory is PlayerWeaponHotbar && (Inventory as PlayerWeaponHotbar).CurWeapon == this);
    }
    public override void OnDrop()
    {
        base.OnDrop();

        Select(false);
        gameObject.SetActive(false);
        transform.SetParent(null);
    }
    #endregion

    #region Method: Select Weapon
    public void Select(bool select)
    {
        IsSelected = select;
        spriteRoot.SetActive(select);
    }
    #endregion
}
#endregion

#region Gun Base
public abstract class GunController<TGun> : WeaponController<TGun> where TGun : GunItem { }
public abstract class GunItem : WeaponItem
{
    #region Var: Stats
    // Timer
    public TimerStat shootTimer = new TimerStat();
    public TimerStat reloadTimer = new TimerStat();
    public TimerStat swapMagazineTimer = new TimerStat();

    // Bullet Data
    public ProjectileData bulletData;

    [Header("Ammo Data")]
    public int loadedBullets;
    public bool isBulletLoaded;
    public IntStat magazineSize;
    #endregion

    #region Var: Anim Clip Names
    public readonly string ANIM_Idle = "Idle";
    public readonly string ANIM_Shoot = "Shoot";
    public readonly string ANIM_Reload = "Reload";
    public readonly string ANIM_SwapMagazine = "SwapMagazine";
    #endregion

    #region Method: Unity
    protected override void Start()
    {
        base.Start();

        #region Init: Stats
        // Init Timer
        shootTimer.SetTick(gameObject);
        shootTimer.ToEnd();
        reloadTimer.SetTick(gameObject);
        swapMagazineTimer.SetTick(gameObject);

        // Init Ammo
        loadedBullets = magazineSize.Value;
        #endregion
    }
    #endregion
}
#endregion

#region Bow Base
public abstract class BowController<TBow> : WeaponController<TBow> where TBow : BowItem { }
public abstract class BowItem : WeaponItem
{
    #region Var: Inspector
    [Header("Visual")]
    [SerializeField] protected GameObject arrowVisual;
    #endregion

    #region Var: Stats
    // Timer
    public TimerStat shootTimer = new TimerStat();
    public TimerStat drawTimer = new TimerStat();

    // Arrow Data
    public ProjectileData arrowData;
    #endregion

    #region Var: Bow Data
    [HideInInspector] public float drawPower = 0;
    #endregion

    #region Var: Anim Clip Names
    public readonly string ANIM_Idle = "Idle";
    public readonly string ANIM_Shoot = "Shoot";
    public readonly string ANIM_Draw = "Pull";
    #endregion

    #region Var: Properties
    public GameObject ArrowVisual => arrowVisual;
    #endregion

    #region Method: Unity
    protected override void Start()
    {
        base.Start();

        #region Init: Stats
        // Init Timer
        shootTimer.SetTick(gameObject);
        shootTimer.ToEnd();
        drawTimer.SetTick(gameObject);
        #endregion
    }
    #endregion
}
#endregion
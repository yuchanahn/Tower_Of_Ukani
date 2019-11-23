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

public abstract class WeaponItem : Item 
{
    public enum WeaponTag
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
    [SerializeField] private WeaponTag[] weaponTags;
    #endregion

    #region Var: Properties
    public GameObject SpriteRoot => spriteRoot;
    public float PivotPointY => pivotPointY;
    public WeaponTag[] WeaponTags => weaponTags;
    public bool IsSelected
    { get; protected set; } = false;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        InitStats();
        Select(false);
    }
    #endregion

    #region Method: Initialize
    public abstract void InitStats();
    #endregion

    #region Method Override: Add/Remove
    public override void OnAdd()
    {
        Select(WeaponHotbar.CurSelected == this);
    }
    public override void OnRemove()
    {
        base.OnRemove();

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
    [Header("Timer")]
    public TimerStat shootTimer;
    public TimerStat reloadTimer;
    public TimerStat swapMagazineTimer;

    [Header("Bullet Data")]
    public WeaponProjectileData bulletData;

    [Header("Ammo Data")]
    public IntStat magazineSize;
    public int loadedBullets;
    public bool isBulletLoaded;
    #endregion

    #region Var: Properties
    // Animation Clip Names
    public string ANIM_Idle
    { get; private set; }
    public string ANIM_Shoot
    { get; private set; }
    public string ANIM_Reload
    { get; private set; }
    public string ANIM_SwapMagazine
    { get; private set; }
    #endregion

    #region Method: Unity
    protected virtual void Start()
    {
        #region Init: Stats
        // Init Timer
        shootTimer.Init(gameObject);
        reloadTimer.Init(gameObject);
        swapMagazineTimer.Init(gameObject);

        // Init Ammo
        loadedBullets = magazineSize.Value;
        #endregion

        #region Init: Animation Clip Names
        ANIM_Idle = string.Concat(info.NameTrimed, "_Idle");
        ANIM_Shoot = string.Concat(info.NameTrimed, "_Shoot");
        ANIM_Reload = string.Concat(info.NameTrimed, "_Reload");
        ANIM_SwapMagazine = string.Concat(info.NameTrimed, "_SwapMagazine");
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
    [Header("Timer")]
    public TimerStat shootTimer;
    public TimerStat drawTimer;

    [Header("Arrow Data")]
    public WeaponProjectileData arrowData;
    #endregion

    #region Var: Bow Data
    [HideInInspector] public float drawPower = 0;
    #endregion

    #region Var: Properties
    public GameObject ArrowVisual => arrowVisual;

    // Animation Clip Names
    public string ANIM_Idle
    { get; private set; }
    public string ANIM_Shoot
    { get; private set; }
    public string ANIM_Draw
    { get; private set; }
    #endregion

    #region Method: Unity
    protected virtual void Start()
    {
        #region Init: Stats
        // Init Timer
        shootTimer.Init(gameObject);
        drawTimer.Init(gameObject);
        #endregion

        #region Init: Animation Clip Names
        ANIM_Idle = string.Concat(info.NameTrimed, "_Idle");
        ANIM_Shoot = string.Concat(info.NameTrimed, "_Shoot");
        ANIM_Draw = string.Concat(info.NameTrimed, "_Pull");
        #endregion
    }
    #endregion
}
#endregion
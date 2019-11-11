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

    #region Method: Item
    public override void Init()
    {
        base.Init();

        // Init Tags
        if (weaponTags != null)
        {
            for (int i = 0; i < weaponTags.Length; i++)
                WeaponTags.Add(weaponTags[i]);
        }

        InitStats();
        Select(false);
    }
    public abstract void InitStats();

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

    #region Var: Properties
    // Animation Clip Names
    public string ANIM_Idle { get; private set; }
    public string ANIM_Shoot { get; private set; }
    public string ANIM_Reload { get; private set; }
    public string ANIM_SwapMagazine { get; private set; }
    #endregion

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
}
#endregion

#region Bow
public abstract class BowController<TBow> : WeaponController<TBow> where TBow : BowItem { }
public abstract class BowItem : WeaponItem
{
    [Header("Visual")]
    [SerializeField] protected GameObject arrowVisual;

    [Header("Timer")]
    public TimerStat shootTimer;
    public TimerStat drawTimer;

    [Header("Arrow Data")]
    public WeaponProjectileData arrowData;

    // Extra Info
    [HideInInspector] public float drawPower = 0;

    #region Var: Properties
    public GameObject ArrowVisual => arrowVisual;

    // Animation Clip Names
    public string ANIM_Idle { get; private set; }
    public string ANIM_Shoot { get; private set; }
    public string ANIM_Draw { get; private set; }
    #endregion

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
}
#endregion
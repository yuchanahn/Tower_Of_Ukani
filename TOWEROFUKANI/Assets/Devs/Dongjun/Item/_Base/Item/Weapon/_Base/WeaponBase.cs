using UnityEngine;

public abstract class WeaponController_Base : SSM_Main { }
public abstract class WeaponController<T> : WeaponController_Base
    where T : WeaponItem
{
    #region Var: Properties
    protected T weaponItem
    { get; private set; }
    public Animator animator
    { get; private set; }
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
        weaponItem = GetComponent<T>();
        animator = GetComponent<Animator>();
    }
    #endregion

    #region Method: CLA
    protected override void RunConditionLogic()
    {
        if (!weaponItem.IsSelected)
        {
            ChangeState(DefaultState);
            return;
        }

        base.RunConditionLogic();
    }
    #endregion
}

public abstract class WeaponItem : UpgradableItem
{
    #region Var: Inspector
    [Header("Weapon Info")]
    [SerializeField] private GameObject spriteRoot;
    [SerializeField] private float pivotPointY;
    #endregion

    #region Var: Properties
    public GameObject SpriteRoot => spriteRoot;
    public bool IsSelected
    { get; protected set; } = false;

    public Animator animator
    { get; private set; }
    #endregion

    #region Var: Stats
    public AttackData attackData;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    #endregion

    #region Method Override: Add / Move / Drop
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        gameObject.SetActive(true);
        transform.SetParent(GM.PlayerObj.transform);
        transform.localPosition = new Vector2(0, pivotPointY);
        CheckSelect();
    }
    public override void OnMove()
    {
        CheckSelect();
    }
    public override void OnDrop()
    {
        base.OnDrop();

        gameObject.SetActive(false);
        transform.SetParent(null);
        Select(false);
    }
    #endregion

    #region Method: Select Weapon
    private void CheckSelect()
    {
        Select(Inventory is PlayerWeaponHotbar && (Inventory as PlayerWeaponHotbar).CurWeapon == this);
    }
    public void Select(bool select)
    {
        IsSelected = select;
        spriteRoot.SetActive(select);
    }
    #endregion
}

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
        weaponItem = GetComponent<T>();
        animator = GetComponent<Animator>();
        base.Awake();
    }
    #endregion

    #region Method: SSM
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

    #region Method: Stats
    public abstract void ResetStats();
    #endregion

    #region Method: Item
    public override void AddLevel(int amount = 1)
    {
        base.AddLevel(amount);

        PlayerStatMod.Apply_Weapon(this);
    }
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        // Attach to Player
        gameObject.SetActive(true);
        transform.SetParent(GM.PlayerObj.transform);
        transform.localPosition = new Vector2(0, pivotPointY);

        // Select
        SelectInActiveSlot();

        // Apply Stat Mod
        PlayerStatMod.Apply_Weapon(this);
    }
    public override void OnMove()
    {
        SelectInActiveSlot();
    }
    public override void OnDrop()
    {
        base.OnDrop();

        // Detach from Player
        gameObject.SetActive(false);
        transform.SetParent(null);

        // Unselect
        Select(false);

        // Reset Stats
        ResetStats();
    }
    #endregion

    #region Method: Select Weapon
    public void Select(bool select)
    {
        IsSelected = select;
        spriteRoot.SetActive(select);
    }
    private void SelectInActiveSlot()
    {
        Select(Inventory is PlayerWeaponHotbar && (Inventory as PlayerWeaponHotbar).CurWeapon == this);
    }
    #endregion
}

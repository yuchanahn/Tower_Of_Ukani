using UnityEngine;

public abstract class WeaponController_Base : CLA_Main { }
public abstract class WeaponController<T> : WeaponController_Base
    where T : WeaponItem
{
    #region Var: Weapon Item
    protected T weaponItem;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
        weaponItem = GetComponent<WeaponItem>() as T;
    }
    #endregion

    #region Method: CLA
    protected override void RunConditionLogic()
    {
        if (!weaponItem.IsSelected)
        {
            ChangeAction(DefaultAction);
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

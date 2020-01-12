using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

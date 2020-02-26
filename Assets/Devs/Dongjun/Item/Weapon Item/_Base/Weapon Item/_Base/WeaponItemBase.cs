using UnityEngine;

public abstract class WeaponItem : UpgradableItem
{
    #region Var: Inspector
    [Header("Weapon Info")]
    [SerializeField] private GameObject spriteRoot;
    [SerializeField] private float pivotPointY;
    #endregion

    #region Prop: 
    public bool IsSelected
    { get; protected set; } = false;

    public GameObject SpriteRoot => spriteRoot;
    #endregion

    #region Var: Stats
    public AttackData AttackData;
    #endregion

    #region Method: Stats
    public virtual void ResetStats()
    {
        AttackData.Reset();
    }
    #endregion

    #region Method: Item
    public override void AddLevel(int amount = 1)
    {
        base.AddLevel(amount);

        // Apply Stat Mod
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
    protected override void OnRemovedFromInventory()
    {
        // Unselect
        Select(false);

        // Detach from Player
        transform.SetParent(null);

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

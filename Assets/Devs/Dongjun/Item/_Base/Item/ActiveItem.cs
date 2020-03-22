using UnityEngine;

public abstract class ActiveItem : UpgradableItem
{
    #region Prop: 
    public bool IsActive
    { get; protected set; } = false;
    public bool CanActivate => CooldownTimer.IsEnded && !IsActive;

    public TimerData CooldownTimer
    { get; private set; } = new TimerData();
    public FloatStat ManaUsage
    { get; protected set; } = new FloatStat(0, min: 0);
    #endregion

    #region Method Override: Add / Drop
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        // Init Cooldown Timer
        CooldownTimer.SetTick(gameObject);

        // Reset Cooldown When Added to Hotbar
        if (inventory is PlayerActiveHotbar)
        {
            CooldownTimer.SetActive(true);
            CooldownTimer.Reset();
        }
        // Stop Cooldown When Added to Inventory
        else
        {
            CooldownTimer.SetActive(false);
            Deactivate();
        }
    }
    protected override void OnRemovedFromInventory()
    {
        // Deactivate Item
        Deactivate();

        // Stop Cooldown Timer
        CooldownTimer.SetActive(false);
        CooldownTimer.Reset();
    }
    #endregion

    #region Method: Active Item
    public void Activate()
    {
        if (!CanActivate
         || !PlayerStats.Inst.UseMana(ManaUsage.Value))
            return;

        IsActive = true;
        LockItemSlot = true;
        OnActivate();
    }
    protected abstract void OnActivate();

    public virtual void Deactivate()
    {
        IsActive = false;
        LockItemSlot = false;
    }
    #endregion
}

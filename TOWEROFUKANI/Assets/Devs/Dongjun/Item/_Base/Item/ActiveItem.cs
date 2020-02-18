using UnityEngine;

public abstract class ActiveItem : UpgradableItem
{
    #region Prop: 
    public TimerData cooldownTimer
    { get; private set; } = new TimerData();
    public bool IsActive
    { get; protected set; } = false;
    public bool CanActivate => cooldownTimer.IsEnded && !IsActive;
    #endregion

    #region Method Override: Add / Drop
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        // Init Cooldown Timer
        cooldownTimer.SetTick(gameObject);

        // Reset Cooldown When Added to Hotbar
        if (inventory is PlayerActiveHotbar)
        {
            cooldownTimer.SetActive(true);
            cooldownTimer.Reset();
        }
        // Stop Cooldown When Added to Inventory
        else
        {
            cooldownTimer.SetActive(false);
            Deactivate();
        }
    }
    protected override void OnRemovedFromInventory()
    {
        // Stop Cooldown Timer
        cooldownTimer.SetActive(false);
        cooldownTimer.Reset();

        // Deactivate Item
        Deactivate();
    }
    #endregion

    #region Method: Active Item
    public void Activate()
    {
        if (!CanActivate)
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

using UnityEngine;

public abstract class ActiveItem : UpgradableItem
{
    #region Var: Properties
    public TimerData cooldownTimer
    { get; protected set; } = new TimerData();
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
            cooldownTimer.Restart();
        }
        // Stop Cooldown When Added to Inventory
        else
        {
            cooldownTimer.SetActive(false);
            Deactivate();
        }
    }
    public override void OnDrop()
    {
        base.OnDrop();

        // Stop Cooldown Timer
        cooldownTimer.SetTick(gameObject, TickType.None);
        cooldownTimer.ToZero();

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
        IsLocked = true;
        OnActivate();
    }
    protected abstract void OnActivate();

    public virtual void Deactivate()
    {
        IsActive = false;
        IsLocked = false;
    }
    #endregion
}

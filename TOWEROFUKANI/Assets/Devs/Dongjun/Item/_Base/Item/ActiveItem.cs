﻿using UnityEngine;

public abstract class ActiveItem : Item
{
    public bool IsActive
    { get; protected set; } = false;
    public TimerData cooldownTimer
    { get; protected set; } = new TimerData();
    public bool CanActivate => cooldownTimer.IsEnded && !IsActive;

    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        cooldownTimer.SetTick(gameObject);
        cooldownTimer.Restart();
    }
    public override void OnRemove()
    {
        base.OnRemove();

        Deactivate();
        cooldownTimer.SetTick(gameObject, TickType.None);
        cooldownTimer.ToZero();
    }

    public abstract void Activate();
    public virtual void Deactivate()
    {
        IsActive = false;
    }
}
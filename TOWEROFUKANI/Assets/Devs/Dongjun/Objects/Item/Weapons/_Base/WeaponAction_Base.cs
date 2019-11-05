using UnityEngine;

public abstract class WeaponAction_Base<TItem> : CLA_Animator
    where TItem : WeaponItem
{
    protected TItem weapon;

    protected override void Awake()
    {
        base.Awake();
        weapon = GetComponent<TItem>();
    }
}

public abstract class GunAction_Base<TItem> : WeaponAction_Base<TItem>
    where TItem : GunItem
{ }

public abstract class BowAction_Base<TItem> : WeaponAction_Base<TItem>
    where TItem : BowItem
{ }

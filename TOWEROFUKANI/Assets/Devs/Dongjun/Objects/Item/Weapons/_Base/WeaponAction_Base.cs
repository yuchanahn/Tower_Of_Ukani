using UnityEngine;

public abstract class WeaponAction_Base<TMain> : MonoAnimator
    where TMain : Weapon
{
    protected TMain weapon;

    protected override void Awake()
    {
        base.Awake();
        weapon = GetComponent<TMain>();
    }
}

public abstract class GunAction_Base<TMain> : WeaponAction_Base<TMain>
    where TMain : Gun { }

public abstract class BowAction_Base<TMain> : WeaponAction_Base<TMain>
    where TMain : Bow { }

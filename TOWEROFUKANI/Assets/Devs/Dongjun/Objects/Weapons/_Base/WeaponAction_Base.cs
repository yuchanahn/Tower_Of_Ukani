using UnityEngine;

public abstract class GunAction_Base<TMain> : MonoAnimator
    where TMain : Gun
{ 
    protected TMain gun;

    protected override void Awake()
    {
        base.Awake();
        gun = GetComponent<TMain>();
    }
}

public abstract class BowAction_Base<TMain> : MonoAnimator
    where TMain : Bow
{
    protected TMain bow;

    protected override void Awake()
    {
        base.Awake();
        bow = GetComponent<TMain>();
    }
}

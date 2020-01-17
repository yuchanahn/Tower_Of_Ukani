using UnityEngine;

public abstract class WeaponAction_Base<T> : CLA_Action_Animator
    where T : WeaponItem
{
    #region Var: Weapon Item
    protected T weapon;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
        weapon = GetComponent<T>();
    }
    #endregion
}

public abstract class GunAction_Base<T> : WeaponAction_Base<T>
    where T : GunItem { }

public abstract class BowAction_Base<T> : WeaponAction_Base<T>
    where T : BowItem { }

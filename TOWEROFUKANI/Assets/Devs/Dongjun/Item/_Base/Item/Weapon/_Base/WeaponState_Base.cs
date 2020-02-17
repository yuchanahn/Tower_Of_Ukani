using UnityEngine;

public abstract class WeaponState_Base<T> : SSM_State
    where T : WeaponItem
{
    #region Var: Weapon Item
    protected T weapon;
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        weapon = GetComponent<T>();
    }
    #endregion
}

public abstract class Melee_State_Base<T> : WeaponState_Base<T>
    where T : MeleeItem { }

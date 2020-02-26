using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OBB_Controller_Weapon<D, W> : OBB_Controller<D, Weapon_State_Base<D, W>>
    where D : OBB_Data_Animator, new()
    where W : WeaponItem
{
    protected W weaponItem;

    protected override void Awake()
    {
        weaponItem = GetComponent<W>();
        base.Awake();
    }

    protected override void GetState<TState>(ref TState state)
    {
        base.GetState(ref state);
        (state as IOBB_State_Weapon).InitWeaponData(weaponItem);
    }
}

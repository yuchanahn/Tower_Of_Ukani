using UnityEngine;

public interface IOBB_State_Weapon : IOBB_State
{
    void InitWeaponData(WeaponItem weaponItem);
}
public abstract class Weapon_State_Base<D, W> : OBB_State, IOBB_State_Weapon
    where D : OBB_Data_Animator
    where W : WeaponItem
{
    protected D data
    { get; private set; }
    protected W weaponItem
    { get; private set; }

    public override void InitData(OBB_Data data)
    {
        this.data = data as D;
    }
    public void InitWeaponData(WeaponItem weaponItem)
    {
        this.weaponItem = weaponItem as W;
    }
}

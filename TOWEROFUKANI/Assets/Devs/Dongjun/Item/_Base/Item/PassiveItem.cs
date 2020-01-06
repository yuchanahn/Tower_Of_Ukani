using UnityEngine;

public abstract class PassiveItem : Item
{
    [Header("God")]
    [SerializeField] private TowerOfUkani.Gods god;

    public TowerOfUkani.Gods God => god;

    protected abstract void SetBonusStats(WeaponItem weapon);
    public void ApplyBonusStats()
    {
        //// Weapon Hotbar
        //for (int i = 0; i < WeaponHotbar.SLOT_SIZE; i++)
        //{
        //    if (WeaponHotbar.Weapons[i] == null)
        //        continue;

        //    SetBonusStats(WeaponHotbar.Weapons[i]);
        //}

        //// Inventory
        //for (int i = 0; i < Inventory.Items.Length; i++)
        //{
        //    if (Inventory.Items[i] == null)
        //        continue;

        //    if (Inventory.Items[i] is WeaponItem)
        //        SetBonusStats(Inventory.Items[i] as WeaponItem);
        //}
    }
}

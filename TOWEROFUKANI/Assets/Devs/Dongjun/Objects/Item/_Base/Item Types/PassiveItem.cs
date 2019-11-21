using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveItem : Item
{
    public enum Gods
    {
        Ukani,
        Bazik,
        Ellai,
        None
    }

    [SerializeField] private WeaponItem.WeaponTag[] affectedWeapons;
    [SerializeField] private Gods god;

    public int Cur_Count
    { get; set; } = 0;
    public int Max_Count
    { get; protected set; } = 1;
    public WeaponItem.WeaponTag[] AffectedWeapons => affectedWeapons;
    public Gods God => god;

    public override void OnRemove()
    {
        // Activate Dropped Item
        DroppedItem.Activate();
    }

    public void ApplyBonusStats()
    {
        // Weapon Hotbar
        for (int i = 0; i < WeaponHotbar.SLOT_SIZE; i++)
        {
            if (WeaponHotbar.Weapons[i] == null)
                continue;

            SetBonusStats(WeaponHotbar.Weapons[i]);
        }

        // Inventory
        //for (int i = 0; i < Inventory.ItemSlot.Items.Length; i++)
        //{
        //    if (Inventory.ItemSlot.Items[i] == null)
        //        continue;

        //    SetBonusStats(Inventory.ItemSlot.Items[i]);
        //}
    }
    protected abstract void SetBonusStats(Item item);
}

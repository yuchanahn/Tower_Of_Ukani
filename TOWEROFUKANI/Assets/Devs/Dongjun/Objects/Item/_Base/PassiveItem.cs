using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveItem : Item
{
    [SerializeField]
    private WeaponItem.WeaponTag[] affectedWeapons;

    public int Cur_Count
    { get; set; } = 0;
    public int Max_Count
    { get; protected set; } = 1;
    public HashSet<WeaponItem.WeaponTag> AffectedWeapons
    { get; private set; } = new HashSet<WeaponItem.WeaponTag>();

    public override void Init()
    {
        base.Init();

        if (affectedWeapons != null)
        {
            for (int i = 0; i < affectedWeapons.Length; i++)
                AffectedWeapons.Add(affectedWeapons[i]);
        }
    }

    public override void OnRemove() 
    { 
        // 패시브 아이템은 버릴 수 없음!
    }

    public void ApplyBonusStats()
    {
        for (int i = 0; i < Inventory.ItemSlot.Items.Length; i++)
        {
            if (Inventory.ItemSlot.Items[i] == null)
                continue;

            SetBonusStats(Inventory.ItemSlot.Items[i]);
        }
    }
    protected abstract void SetBonusStats(Item item);
}

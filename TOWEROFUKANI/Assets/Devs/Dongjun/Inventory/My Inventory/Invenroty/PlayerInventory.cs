using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : ToU_Inventory
{
    protected override void Awake()
    {
        Init(20, typeof(WeaponItem), typeof(ActiveItem));
    }
}

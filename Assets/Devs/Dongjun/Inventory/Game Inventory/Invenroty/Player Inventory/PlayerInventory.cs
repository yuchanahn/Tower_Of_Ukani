using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInventory : ToU_Inventory
{
    protected override void Awake()
    {
        // Get UI
        inventoryUI = InGameUI_Manager.Inst.inventoryUI;

        // Init
        Init(20,
            typeof(ResourceItem),
            typeof(WeaponItem),
            typeof(ActiveItem),
            typeof(ConsumableItem));
    }
}

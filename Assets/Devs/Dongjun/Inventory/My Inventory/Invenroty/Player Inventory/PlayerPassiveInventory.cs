using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassiveInventory : ToU_Inventory
{
    protected override void Awake()
    {
        // Get UI
        //inventoryUI = InGameUI_Manager.Inst.inventoryUI;

        // Init
        Init(20, typeof(PassiveItem));
    }
}

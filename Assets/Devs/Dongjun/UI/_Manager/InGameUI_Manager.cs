using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI_Manager : SingletonBase<InGameUI_Manager>
{
    [Header("Inventory")]
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private WeaponHotbarUI _weaponHotbarUI;
    [SerializeField] private ActiveHotbarUI _activeHotbarUI;
    [SerializeField] private ConsumableHotbarUI _consumableHotbarUI;

    public InventoryUI inventoryUI => _inventoryUI;
    public WeaponHotbarUI weaponHotbarUI => _weaponHotbarUI;
    public ActiveHotbarUI activeHotbarUI => _activeHotbarUI;
    public ConsumableHotbarUI consumableHotbarUI => _consumableHotbarUI;

    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = false;
    }
}

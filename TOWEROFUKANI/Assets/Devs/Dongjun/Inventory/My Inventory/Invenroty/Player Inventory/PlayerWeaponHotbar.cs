using UnityEngine;

public class PlayerWeaponHotbar : ToU_Inventory
{
    #region Var: Properties
    public int CurSlot
    { get; private set; } = 0;
    public WeaponItem CurWeapon => items[CurSlot] as WeaponItem;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        inventoryUI = InGameUI_Manager.Inst.weaponHotbarUI;

        Init(3, typeof(WeaponItem));
        (inventoryUI.slotUIs[CurSlot] as WeaponHotbarSlot).Select(true);
    }
    private void Update()
    {
        ChangeWeapon();
        DropWeapon();
    }
    #endregion

    #region Method: Weapon Hotbar
    private void ChangeWeapon()
    {
        const int TO_RIGHT = 1;
        const int TO_LEFT = -1;

        void UpdateHotbar(int dir)
        {
            int prevSlot = CurSlot;

            if (dir == TO_RIGHT) CurSlot = CurSlot < 2 ? CurSlot + 1 : 0;
            if (dir == TO_LEFT) CurSlot = CurSlot > 0 ? CurSlot - 1 : 2;

            (items[prevSlot] as WeaponItem)?.Select(false);
            (items[CurSlot] as WeaponItem)?.Select(true);

            (inventoryUI.slotUIs[prevSlot] as WeaponHotbarSlot).Select(false);
            (inventoryUI.slotUIs[CurSlot] as WeaponHotbarSlot).Select(true);
        }

        // Scroll
        if (Input.mouseScrollDelta.y < 0) UpdateHotbar(TO_RIGHT);
        if (Input.mouseScrollDelta.y > 0) UpdateHotbar(TO_LEFT);
    }
    private void DropWeapon()
    {
        if (Input.GetKeyDown(PlayerActionKeys.DropWeapon))
        {
            DropItem(CurSlot);
        }
    }
    #endregion
}

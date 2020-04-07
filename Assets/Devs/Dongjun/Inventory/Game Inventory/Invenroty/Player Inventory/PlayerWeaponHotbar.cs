using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHotbar : ToU_Inventory
{
    #region Var: Inspector
    [SerializeField] private FistItem fist;
    #endregion

    #region Prop: 
    public BoolSet IsSlotLocked
    { get; private set; } = new BoolSet();

    public int CurSlot
    { get; private set; } = 0;
    public WeaponItem CurWeapon => items[CurSlot] as WeaponItem;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        // Init Inventory
        inventoryUI = InGameUI_Manager.Inst.weaponHotbarUI;
        Init(3, typeof(WeaponItem));

        // Select Slot 0
        (inventoryUI.slotUIs[CurSlot] as WeaponHotbarSlot).Select(true);
    }
    private void Start()
    {
        // On Stun
        PlayerActionEventManager.AddEvent(PlayerActions.Stunned, this.NewPlayerActionEvent(() => LockSlots(this, true)));
        PlayerActionEventManager.AddEvent(PlayerActions.StunEnd, this.NewPlayerActionEvent(() => LockSlots(this, false)));
    }
    private void Update()
    {
        if (PlayerStats.Inst.IsDead)
        {
            fist.Select(false);
            CurWeapon?.Select(false);
            return;
        }

        // Toggle Fist
        fist.Select(CurWeapon == null);

        if (PlayerStatus.IsStunned.Value)
            return;

        ChangeSlot();
        DropWeapon();
    }
    #endregion

    #region Method: Weapon Hotbar
    public void LockSlots<Tthis>(Tthis _this, bool _lock) where Tthis : class
    {
        IsSlotLocked.Set(_this, _lock);

        for (int i = 0; i < inventoryUI.slotUIs.Length; i++)
            (inventoryUI.slotUIs[i] as DroppableSlotUI).IsLocked = IsSlotLocked.Value;
    }

    private void ChangeSlot()
    {
        if (IsSlotLocked.Value)
            return;

        const int TO_RIGHT = 1;
        const int TO_LEFT = -1;

        void UpdateHotbar(int dir)
        {
            int prevSlot = CurSlot;

            // Change Slot
            if (dir == TO_RIGHT) CurSlot = CurSlot < 2 ? CurSlot + 1 : 0;
            if (dir == TO_LEFT) CurSlot = CurSlot > 0 ? CurSlot - 1 : 2;

            // Unlselect Previous
            (items[prevSlot] as WeaponItem)?.Select(false);
            (items[CurSlot] as WeaponItem)?.Select(true);

            // Select Current
            (inventoryUI.slotUIs[prevSlot] as WeaponHotbarSlot).Select(false);
            (inventoryUI.slotUIs[CurSlot] as WeaponHotbarSlot).Select(true);
        }

        // Scroll
        if (Input.mouseScrollDelta.y < 0) UpdateHotbar(TO_RIGHT);
        if (Input.mouseScrollDelta.y > 0) UpdateHotbar(TO_LEFT);
    }
    private void DropWeapon()
    {
        if (Input.GetKeyDown(PlayerActionKeys.DropWeapon) && CurWeapon != null)
            DropItem(CurSlot);
    }
    #endregion
}

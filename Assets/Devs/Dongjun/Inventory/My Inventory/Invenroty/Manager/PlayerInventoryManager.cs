using Dongjun.Helper;

public class PlayerInventoryManager : SingletonBase<PlayerInventoryManager>
{
    private PlayerPassiveInventory _passiveInventory;
    private PlayerInventory _inventory;
    private PlayerWeaponHotbar _weaponHotbar;
    private PlayerActiveHotbar _activeHotbar;
    private PlayerConsumableHotbar _consumableHotbar;

    public static PlayerPassiveInventory passiveInventory => Inst._passiveInventory;
    public static PlayerInventory inventory => Inst._inventory;
    public static PlayerWeaponHotbar weaponHotbar => Inst._weaponHotbar;
    public static PlayerActiveHotbar activeHotbar => Inst._activeHotbar;
    public static PlayerConsumableHotbar consumableHotbar => Inst._consumableHotbar;

    protected override void Awake()
    {
        base.Awake();

        gameObject.GetComponent(ref _inventory);
        gameObject.GetComponent(ref _passiveInventory);
        gameObject.GetComponent(ref _weaponHotbar);
        gameObject.GetComponent(ref _activeHotbar);
        gameObject.GetComponent(ref _consumableHotbar);
    }
}

using UnityEngine;

public class PlayerConsumableHotbar : ToU_Inventory
{
    protected override void Awake()
    {
        // Get UI
        inventoryUI = InGameUI_Manager.Inst.consumableHotbarUI;

        // Init
        Init(4, typeof(ConsumableItem));
    }
    private void Update()
    {
        ConsumeItem();
    }

    private void ConsumeItem()
    {
        if (PlayerStatus.Inst.IsHardCCed || !inventoryUI.gameObject.activeInHierarchy)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ConsumeAndRemove(items[0] as ConsumableItem);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ConsumeAndRemove(items[1] as ConsumableItem);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ConsumeAndRemove(items[2] as ConsumableItem);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            ConsumeAndRemove(items[3] as ConsumableItem);
    }
    private void ConsumeAndRemove(ConsumableItem item)
    {
        if (item == null)
            return;

        if (item.Consume())
            item.Inventory.RemoveItem(item);
    }
}

using UnityEngine;

public class PlayerActiveHotbar : ToU_Inventory
{
    protected override void Awake()
    {
        // Get UI
        inventoryUI = InGameUI_Manager.Inst.activeHotbarUI;

        // Init
        Init(4, typeof(ActiveItem));
    }
    private void Update()
    {
        ActivateItem();
    }

    private void ActivateItem()
    {
        if (PlayerStatus.Inst.IsStunned || !inventoryUI.gameObject.activeInHierarchy)
            return;

        ActiveItem toActivate = null;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            toActivate = items[0] as ActiveItem;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            toActivate = items[1] as ActiveItem;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            toActivate = items[2] as ActiveItem;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            toActivate = items[3] as ActiveItem;

        toActivate?.Activate();
    }
}

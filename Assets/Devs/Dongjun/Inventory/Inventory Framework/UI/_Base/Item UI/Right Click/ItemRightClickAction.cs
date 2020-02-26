using UnityEngine;

public class ItemRightClickAction : MonoBehaviour
{
    [Header("Context Menu")]
    [SerializeField] private Transform itemContextMenuParent;
    [SerializeField] private UI_Screen_ItemContextMenu itemContextMenuPrefab;

    private UI_Screen_ItemContextMenu openedContextMenu;

    private void OnDisable()
    {
        if (openedContextMenu != null)
            openedContextMenu.Close();
    }

    public virtual void OnRightClick(Item item)
    {
        ShowContextMenu(item);
    }

    protected virtual void ShowContextMenu(Item item)
    {
        // Spawn Context Menu
        openedContextMenu = Instantiate(itemContextMenuPrefab, itemContextMenuParent, false);
        openedContextMenu.Open();

        // Set Title
        openedContextMenu.SetTitle(item.Info.ItemName);

        // Spawn Context Menu Buttons
        SetContextMenuButtons(item, openedContextMenu);
    }
    protected virtual void SetContextMenuButtons(Item item, UI_Screen_ItemContextMenu contextMenu)
    {
        if (item is ConsumableItem)
        {
            contextMenu.AddButton("Consume", item as ConsumableItem, contextMenu, (i, contenxtmMenu) =>
                {
                    if (i == null) return;

                    if (i.Consume()) i.Inventory.RemoveItem(i);
                    contextMenu.Close();
                });
        }

        contextMenu.AddButton("Drop", item, contextMenu, (i, contenxtmMenu) =>
        {
            if (i == null) return;

            i.Inventory.DropItem(i.Inventory.GetIndex_Item(i));
            contextMenu.Close();
        });

        contextMenu.AddButton("Remove", item, contextMenu, (i, contenxtmMenu) =>
        {
            if (i == null) return;

            i.Inventory.RemoveItem(i.Inventory.GetIndex_Item(i), i.Info.Count);
            contextMenu.Close();
        });
    }
}

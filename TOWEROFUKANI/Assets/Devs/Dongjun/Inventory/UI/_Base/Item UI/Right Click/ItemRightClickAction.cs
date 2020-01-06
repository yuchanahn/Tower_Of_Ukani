using UnityEngine;

public class ItemRightClickAction : MonoBehaviour
{
    [Header("Context Menu")]
    [SerializeField] private Transform itemContextMenuParent;
    [SerializeField] private UI_Screen_ItemContextMenu itemContextMenuPrefab;

    public virtual void OnRightClick(Item item)
    {
        ShowContextMenu(item);
    }

    protected virtual void ShowContextMenu(Item item)
    {
        // Spawn Context Menu
        UI_Screen_ItemContextMenu contextMenu = Instantiate(itemContextMenuPrefab, itemContextMenuParent, false);
        contextMenu.Open();

        // Set Title
        //contextMenu.SetTitle(item.Name);

        // Spawn Context Menu Buttons
        SetContextMenuButtons(item, contextMenu);
    }
    protected virtual void SetContextMenuButtons(Item item, UI_Screen_ItemContextMenu contextMenu)
    {
        //if (item is ConsumableItem)
        //    contextMenu.AddButton("Consume", item as ConsumableItem, contextMenu, ConsumeItem);

        //contextMenu.AddButton("Drop", item, contextMenu, DropItem);
        //contextMenu.AddButton("Remove", item, contextMenu, RemoveItem);
    }

    //protected virtual void ConsumeItem(ConsumableItem item, UI_Screen_ItemContextMenu contextMenu)
    //{
    //    item.Consume();
    //    item.Inventory.RemoveItem(item, 1);

    //    if (item.Count == 0)
    //        contextMenu.Close();
    //}
    //protected virtual void DropItem(Item item, UI_Screen_ItemContextMenu contextMenu)
    //{
    //    item.Inventory.DropItem(item, 1);

    //    if (item.Count == 0)
    //        contextMenu.Close();
    //}
    //protected virtual void RemoveItem(Item item, UI_Screen_ItemContextMenu contextMenu)
    //{
    //    item.Inventory.RemoveItem(item, 1);

    //    if (item.Count == 0)
    //        contextMenu.Close();
    //}
}

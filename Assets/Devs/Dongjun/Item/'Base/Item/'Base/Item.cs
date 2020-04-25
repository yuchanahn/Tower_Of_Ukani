using System;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    #region Var: Inspector
    [Header("Item Info")]
    [SerializeField] protected ItemInfo info;

    [Header("Dropped Item Prefab")]
    [SerializeField] protected DroppedItem droppedItemPrefab;
    #endregion

    #region Prop: 
    // Item Data
    public ItemInfo Info => info;
    public DroppedItem DroppedItemPrefab => droppedItemPrefab;

    // Inventory
    public InventoryBase Inventory
    { get; private set; }
    public bool LockItemSlot
    { get; set; } = false;
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        // Temp
        if (info.ID == null)
            info.Init();
    }
    #endregion

    #region Method: Data
    public virtual void SetInfo(ItemInfo other)
    {
        info.SetID(other.ID);
        info.Count = other.Count;
    }
    #endregion

    #region Method: Item
    public virtual void OnAdd(InventoryBase inventory)
    {
        Inventory = inventory;
    }

    public virtual void OnMove()
    {

    }

    protected virtual void OnRemovedFromInventory()
    {

    }

    protected virtual DroppedItem SpawnDroppedItem(Vector2 pos)
    {
        DroppedItem droppedItem = Instantiate(DroppedItemPrefab, pos, Quaternion.identity).GetComponent<DroppedItem>();
        droppedItem.SetRef_Item(this);
        return droppedItem;
    }
    public DroppedItem OnDrop()
    {
        OnRemovedFromInventory();
        return SpawnDroppedItem(GM.PlayerPos);
    }
    public DroppedItem OnDrop(Vector2 pos)
    {
        OnRemovedFromInventory();
        return SpawnDroppedItem(pos);
    }

    public void OnRemove()
    {
        OnRemovedFromInventory();
        Destroy(gameObject);
    }
    #endregion
}
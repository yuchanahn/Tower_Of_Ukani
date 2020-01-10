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

    #region Var: Properties
    // Item Data
    public ItemInfo Info => info;
    public DroppedItem DroppedItemPrefab => droppedItemPrefab;

    // Inventory Ref
    public InventoryBase Inventory
    { get; private set; }
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
    public void SetInfo(ItemInfo other)
    {
        info.SetID(other.ID);
        info.Count = other.Count;
    }
    #endregion

    #region Method: Add / Move / Drop / Remove
    public virtual void OnAdd(InventoryBase inventory)
    {
        Inventory = inventory;
    }
    public virtual void OnMove()
    {

    }
    public virtual void OnDrop()
    {
        this.SpawnDroppedItem();
    }
    public virtual void OnRemove()
    {
        Destroy(gameObject);
    }
    #endregion
}
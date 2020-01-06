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
    public ItemInfo Info => info;
    public DroppedItem DroppedItemPrefab => droppedItemPrefab;
    public InventoryBase Inventory
    { get; private set; }
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        info.Init();
    }
    #endregion

    #region Method: Add/Remove
    public virtual void OnAdd(InventoryBase inventory)
    {
        Inventory = inventory;
    }
    public virtual void OnRemove()
    {
        this.SpawnDroppedItem();
    }
    #endregion
}
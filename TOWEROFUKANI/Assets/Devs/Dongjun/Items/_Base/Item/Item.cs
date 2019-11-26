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

    public int Count
    { get; private set; } = 1;
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        info.Init();
    }
    #endregion

    #region Method: Add Count
    public void AddCount(int amount)
    {
        Count += amount;
    }
    #endregion

    #region Method: Add/Remove
    public abstract void OnAdd();
    public virtual void OnRemove()
    {
        this.SpawnDroppedItem();
    }
    #endregion
}
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    #region Var: Inspector
    [SerializeField] protected ItemInfo info;
    #endregion

    #region Var: Properties
    public ItemInfo Info => info;

    public DroppedItem DroppedItem { get; private set; }
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        info.Init();
    }
    #endregion

    #region Method: Item
    public void InitRef_DroppedItem(DroppedItem droppedItem)
    {
        DroppedItem = droppedItem;
    }

    public abstract void OnAdd();
    public abstract void OnRemove();
    #endregion
}
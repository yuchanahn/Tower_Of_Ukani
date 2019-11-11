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

    #region Method: Item

    public void InitRef_DroppedItem(DroppedItem droppedItem)
    {
        DroppedItem = droppedItem;
    }
    public virtual void Init()
    {
        info.Init();
    }

    public abstract void OnAdd();
    public abstract void OnRemove();
    #endregion
}
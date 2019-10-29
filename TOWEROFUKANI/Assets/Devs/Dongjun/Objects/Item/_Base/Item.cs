using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemInfo info;
    [SerializeField] protected DroppedItem droppedItem;

    public ItemInfo Info => info;
    public DroppedItem DroppedItem => droppedItem;

    public abstract void OnAdd();
    public abstract void OnRemove();

    protected virtual void Awake()
    {
        info.Init();
    }

    public void Init(DroppedItem droppedItem)
    {
        this.droppedItem = droppedItem;
    }
}

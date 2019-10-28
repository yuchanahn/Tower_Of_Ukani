using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemInfo info;
    public ItemInfo Info => info;

    public abstract void OnAdd();
    public abstract void OnRemove();

    protected virtual void Awake()
    {
        info.Init();
    }
}

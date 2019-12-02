using UnityEngine;
using UnityEngine.UI;

public abstract class ItemSlot : MonoBehaviour
{
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected Sprite emptySprite;

    public int Index { get; private set; }

    public void Init(int index)
    {
        this.Index = index;
    }
    public abstract void SetData(Item item);
}

using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Inst { get; private set; }
    public List<Item> items { get; private set; } = new List<Item>();


    private void Awake()
    {
        Inst = this;
    }

    public bool ContainsItem(Item item)
    {
        return items.Contains(item);
    }
    public void AddItem(Item item)
    {
        item.OnAdd();
    }
    public void RemoveItem(Item item)
    {
        item.OnRemove();
    }
}

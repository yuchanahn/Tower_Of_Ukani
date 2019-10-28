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

    public bool HasItem(Item item)
    {
        if (items.Count == 0)
            return false;

        for (int i = 0; i < items.Count; i++)
        {
            if (item.Info.Name == items[i].Info.Name)
                return true;
        }
        return false;
    }
    public void AddItem(Item item)
    {
        item.OnAdd();

        if (!HasItem(item))
            items.Add(item);
    }
    public void RemoveItem(Item item)
    {
        item.OnRemove();

        if (items.Contains(item))
            items.Remove(item);
    }
}

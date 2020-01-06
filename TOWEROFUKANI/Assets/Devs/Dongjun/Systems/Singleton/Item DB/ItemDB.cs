using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : SingletonBase<ItemDB>
{
    [SerializeField] private Item[] items;

    public static Dictionary<string, Item> Items
    { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        if (Items == null)
            Items = new Dictionary<string, Item>();
        else
            Items.Clear();

        for (int i = 0; i < items.Length; i++)
        {
            Items.Add(items[i].name, items[i]);
        }
    }

    public static Item Spawn(string name, int count = 1)
    {
        Item item = Instantiate(Items[name].gameObject).GetComponent<Item>();
        item.transform.SetParent(Inst.transform, false);
        item.name = name;

        item.Info.Init();
        item.Info.Count = Mathf.Max(1, count);
        return item;
    }
    public static Item Clone(Item item)
    {
        Item clone = Instantiate(Items[item.Info.ItemName].gameObject).GetComponent<Item>();
        clone.transform.SetParent(Inst.transform, false);
        clone.name = item.Info.ItemName;

        clone.Info.Init();
        clone.Info.SetID(item.Info.ID);
        clone.Info.Count = Mathf.Max(1, item.Info.Count);
        return clone;
    }
}

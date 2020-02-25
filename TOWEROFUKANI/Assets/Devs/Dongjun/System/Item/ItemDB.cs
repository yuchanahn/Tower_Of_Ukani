using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : SingletonBase<ItemDB>
{
    #region Var: Inspector
    [SerializeField] private Item[] items;
    #endregion

    #region Var: Propertis
    public Dictionary<string, Item> Items
    { get; private set; }
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        if (Items == null)
            Items = new Dictionary<string, Item>();
        else
            Items.Clear();

        for (int i = 0; i < items.Length; i++)
        {
            Items.Add(items[i].Info.ItemName, items[i]);
        }
    }
    #endregion

    #region Method: Spawn Item
    public Item SpawnItem(string name, int count = 1)
    {
        Item item = Instantiate(Items[name].gameObject).GetComponent<Item>();
        item.transform.SetParent(Inst.transform, false);
        item.name = name;

        item.Info.Init();
        item.Info.Count = count;
        return item;
    }
    public Item CloneItem(Item item)
    {
        Item clone = Instantiate(Items[item.Info.ItemName].gameObject).GetComponent<Item>();
        clone.transform.SetParent(Inst.transform, false);
        clone.name = item.Info.ItemName;

        clone.SetInfo(item.Info);
        return clone;
    }

    public DroppedItem SpawnDroppedItem(string name, int count = 1)
    {
        Item item = Instantiate(Items[name].gameObject).GetComponent<Item>();
        item.transform.SetParent(Inst.transform, false);
        item.name = name;

        item.Info.Init();
        item.Info.Count = count;

        if (item is WeaponItem)
            item.gameObject.SetActive(false);

        DroppedItem droppedItem = item.SpawnDroppedItem();
        droppedItem.name = $"Dropped {name}";

        return droppedItem;
    }
    #endregion
}

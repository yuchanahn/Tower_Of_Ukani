using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    #region Method: Editor
    public void LoadAllItemPrefabs()
    {
        string[] guids = AssetDatabase.FindAssets("l:Item");

        items = new Item[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            items[i] = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guids[i])).GetComponent<Item>();
        }

        PrefabUtility.RecordPrefabInstancePropertyModifications(this);
    }
    #endregion

    #region Method: Spawn Item
    public Item SpawnItem(string name, int count = 1)
    {
        Item item = Instantiate(Items[name].gameObject).GetComponent<Item>();
        item.Info.Init();
        item.Info.Count = count;

        item.transform.SetParent(Inst.transform, false);
        item.name = name;
        return item;
    }
    public Item CloneItem(Item item)
    {
        Item clone = SpawnItem(item.Info.ItemName, item.Info.Count);
        clone.SetInfo(item.Info);

        clone.name = item.Info.ItemName;
        return clone;
    }

    public DroppedItem SpawnDroppedItem(string name, int count = 1)
    {
        Item item = SpawnItem(name, count);
        return item.OnDrop();
    }
    public DroppedItem SpawnDroppedItem(Vector2 pos, string name, int count = 1)
    {
        Item item = SpawnItem(name, count);
        return item.OnDrop(pos);
    }
    #endregion
}

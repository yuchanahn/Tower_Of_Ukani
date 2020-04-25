using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemDB : SingletonBase<ItemDB>
{
    public enum SpawnMode
    {
        Clone,
        Fresh
    }

    #region Var: Inspector
    [SerializeField] private Item[] items;
    #endregion

    #region Var: Propertis
    public Dictionary<Type, Item> Items
    { get; private set; }
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        if (Items == null)
            Items = new Dictionary<Type, Item>();
        else
            Items.Clear();

        for (int i = 0; i < items.Length; i++)
        {
            Items.Add(items[i].GetType(), items[i]);
        }
    }
    #endregion

    #region Method: Editor
#if UNITY_EDITOR
    public void LoadAllItemPrefabs()
    {
        string[] guids = AssetDatabase.FindAssets("l:Item");

        List<Item> foundItems = new List<Item>();
        for (int i = 0; i < guids.Length; i++)
        {
            var item = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guids[i])).GetComponent<Item>();
            if (item != null)
                foundItems.Add(item);
        }

        items = foundItems.ToArray();

        PrefabUtility.RecordPrefabInstancePropertyModifications(this);
    }
#endif
    #endregion

    #region Method: Get Item
    public Item GetItemPrefab<T>() where T: Item
    {
        return Items[typeof(T)];
    }
    #endregion

    #region Method: Spawn Item
    public Item SpawnItem<T>(int count = 1) where T : Item
    {
        Item item = Instantiate(Items[typeof(T)].gameObject).GetComponent<Item>();
        item.Info.Init();
        item.Info.Count = count;

        item.transform.SetParent(Inst.transform, false);
        item.name = name;
        return item;
    }
    public Item SpawnItem(Type itemType, int count = 1)
    {
        Item item = Instantiate(Items[itemType].gameObject).GetComponent<Item>();
        item.Info.Init();
        item.Info.Count = count;

        item.transform.SetParent(Inst.transform, false);
        item.name = name;
        return item;
    }
    public Item SpawnCloneItem(Item item)
    {
        Item clone = SpawnItem(item.GetType(), item.Info.Count);
        clone.name = item.Info.ItemName;

        // Clone Info
        clone.SetInfo(item.Info);
        
        // Clone Level
        if (item as UpgradableItem != null)
        {
            UpgradableItem upgradableItem = item as UpgradableItem;
            UpgradableItem upgradableClone = clone as UpgradableItem;
            upgradableClone.SetLevel(upgradableItem.ItemLevel);
        }
        return clone;
    }

    public DroppedItem SpawnDroppedItem<T>(Vector2 pos, int count = 1) where T : Item
    {
        return SpawnItem<T>(count).OnDrop(pos);
    }
    public DroppedItem SpawnDroppedItem(Item item, Vector2 pos, int count = 1, SpawnMode spawnMode = SpawnMode.Fresh)
    {
        if (spawnMode == SpawnMode.Fresh)
            return SpawnItem(item.GetType(), count).OnDrop(pos);
        else
            return SpawnCloneItem(item).OnDrop(pos);
    }
    #endregion
}

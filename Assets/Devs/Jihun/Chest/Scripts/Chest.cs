﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviour
{
    [SerializeField] Transform itemSpawnPoint;

    public bool isOpened = false;

    public Transform slotRoot;
    public Transform chestSelectOption;

    [SerializeField] private ChestItemSlot slotPrefab;

    List<ChestItemSlot> slots = new List<ChestItemSlot>();

    
    [HideInInspector] public ChestItem selectedItem = null;

    private void Awake()
    {
        if (gameObject.scene.name == "LevelEditor") enabled = false;
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        selectedItem = null;
        SetNewItem("Machinegun");
        SetNewItem("Wooden Shortbow");
        SetNewItem("Potion of Healing", 5);
    }

    //슬롯 오브젝트를 레이아웃 그룹에 생성하고 프로퍼티 설정
    void SetNewItem(string name, int count = 1)
    {
        ChestItemSlot slot = Instantiate<ChestItemSlot>(slotPrefab);

        slots.Add(slot);

        slot.transform.SetParent(slotRoot);
        slot.transform.localScale = Vector3.one;

        slot.SetItem(new ChestItem(ItemDB.Inst.Items[name].Info, count), this);
    }

    public void SelectItem(ChestItemSlot select)
    {
        foreach (ChestItemSlot slot in slots)
        {
            slot.transform.localScale = Vector3.one;
        }
        select.transform.localScale = Vector3.one * 1.1f;
        selectedItem = select.item;
    }

    public void OpenChest()
    {
        chestSelectOption.gameObject.SetActive(true);
    }

    public void CloseChest()
    {
        chestSelectOption.gameObject.SetActive(false);
    }

    public void ToggleChest()
    {
        if (chestSelectOption.gameObject.activeSelf) CloseChest();
        else OpenChest();
    }

    public void SpawnItem()
    {
        if (selectedItem == null) return;

        ItemDB.Inst.SpawnDroppedItem(itemSpawnPoint.position, selectedItem.info.ItemName, selectedItem.count);
        isOpened = true;
        ChestManager.Inst.CloseChest();
    }
}

[System.Serializable]
public class ChestItem
{
    public ItemInfo info;
    public int count;

    public ChestItem() { }
    public ChestItem(ItemInfo info, int count)
    {
        this.info = info;
        this.count = count;
    }
}

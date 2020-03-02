using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] Transform itemSpawnPoint;

    public bool isOpened = false;

    public Transform slotRoot;
    public Transform chestSelectOption;

    [SerializeField] private ChestItemSlot slotPrefab;

    List<ChestItemSlot> slots = new List<ChestItemSlot>();

    public ChestItem selectedItem = null;

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

    private void Update()
    {
        bool isCloseToPlayer = (ShopManager.Inst.shopRange > Vector2.Distance(transform.position, GM.PlayerPos));

        //상자 오픈
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //거리 확인
            if (!isCloseToPlayer) return;
            //열어봤었는지 확인
            if (isOpened) return;
            ToggleChest();
        }
        if (!isCloseToPlayer) CloseChest();
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
        CloseChest();
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

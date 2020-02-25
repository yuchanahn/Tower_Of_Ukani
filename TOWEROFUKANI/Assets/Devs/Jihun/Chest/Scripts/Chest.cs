using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool isOpened = false;

    public Transform slotRoot;
    public Transform chestSelectOption;

    [SerializeField] private ChestItemSlot slotPrefab;

    List<ChestItemSlot> slots = new List<ChestItemSlot>();

    public ChestItem selectedItem;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        SetNewItem("Machinegun", 1);
        SetNewItem("Wooden Shortbow", 2);
        SetNewItem("Potion of Healing", 5);
    }

    //슬롯 오브젝트를 레이아웃 그룹에 생성하고 프로퍼티 설정
    void SetNewItem(string name, int count)
    {
        ChestItemSlot slot = Instantiate<ChestItemSlot>(slotPrefab);

        slot.transform.SetParent(slotRoot);
        slot.transform.localScale = Vector3.one;

        slot.SetItem(new ChestItem(ItemDB.Inst.Items[name].Info, count), this);
    }

    private void Update()
    {
        //상자 오픈
        if(Input.GetKeyDown(KeyCode.Z))
        {
            //거리 확인
            if (ShopManager.Inst.shopRange < Vector2.Distance(transform.position, GM.PlayerPos)) return;
            //열어봤었는지 확인
            if (isOpened) return;
            OpenChest();
        }
    }

    public void OpenChest()
    {
        chestSelectOption.gameObject.SetActive(true);
    }

    public void CloseChest()
    {
        chestSelectOption.gameObject.SetActive(false);
    }

    public void SpawnItem()
    {
        ItemDB.Inst.SpawnDroppedItem(transform.position, selectedItem.info.ItemName, selectedItem.count);
        isOpened = true;
        CloseChest();
    }
}

[System.Serializable]
public struct ChestItem
{
    public ItemInfo info;
    public int count;

    public ChestItem(ItemInfo info, int count)
    {
        this.info = info;
        this.count = count;
    }
}

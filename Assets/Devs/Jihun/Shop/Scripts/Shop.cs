using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractiveObj
{
    [SerializeField] Transform shopUI;
    [SerializeField] Transform itemSpawnPoint;


    public Transform slotRoot;

    [SerializeField] private ShopSlot slotPrefab;

    private List<ShopSlot> slots = new List<ShopSlot>();

    private void Start()
    {
        Init();
    }


    Item GetItem<T>() where T: Item
    {
        return ItemDB.Inst.GetItemPrefab<T>();
    }

    // 슬롯들을 리스트에 넣어줌.
    void Init()
    {
        Item i1 = GetItem<MachineGunItem>();
        SellNewItem(i1, 2);
        SellNewItem(GetItem<Gold>());

        int slotCnt = slotRoot.childCount;

        for(int i = 0; i < slotCnt; i++)
        {
            ShopSlot slot = slotRoot.GetChild(i).GetComponent<ShopSlot>();
            slot.SetSpawnPos(itemSpawnPoint.position);

            slots.Add(slot);
        }

       //Debug.Log( (slotRoot.gameObject.scene.IsValid()) .ToString()) ;
    }

    // 판매 아이템 목록에 아이템 추가.
    public void SellNewItem(Item item, int level = 0, params ItemCost[] itemCosts)
    {
        ShopSlot slot = Instantiate<ShopSlot>(slotPrefab);

        slot.transform.SetParent(slotRoot);
        slot.transform.localScale = Vector3.one;

        slot.SetItem(item, level, itemCosts);
    }

    public override InteractiveObj Interact()
    {
        if(shopUI.gameObject.activeSelf)
        {
            CloseShop();
            return null;
        }
        else
        {
            OpenShop();
            return this;
        }
    }

    public void CloseShop()
    {
        shopUI.gameObject.SetActive(false);
    }
    public void OpenShop()
    {
        shopUI.gameObject.SetActive(true);
    }
}

public class ItemCost
{
    public Item item;
    public int price;

    public ItemCost(Item item, int price = 0)
    {
        this.item = item;
        this.price = price;
    }
}
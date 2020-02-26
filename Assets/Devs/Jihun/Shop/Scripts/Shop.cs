using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Transform slotRoot;

    [SerializeField] private ShopSlot slotPrefab;

    private List<ShopSlot> slots = new List<ShopSlot>();

    private void Start()
    {
        Init();
    }

    // 슬롯들을 리스트에 넣어줌.
    void Init()
    {
        SellNewItem("Overlord");
        SellNewItem("Machinegun");
        SellNewItem("Phantom Pistol");
        SellNewItem("Equilibrium");


        int slotCnt = slotRoot.childCount;

        for(int i = 0; i < slotCnt; i++)
        {
            ShopSlot slot = slotRoot.GetChild(i).GetComponent<ShopSlot>();

            slots.Add(slot);
        }

       //Debug.Log( (slotRoot.gameObject.scene.IsValid()) .ToString()) ;
    }

    // 판매 아이템 목록에 아이템 추가.
    public void SellNewItem(string name, params ItemCost[] itemCosts)
    {
        ShopSlot slot = Instantiate<ShopSlot>(slotPrefab);

        slot.transform.SetParent(slotRoot);
        slot.transform.localScale = Vector3.one;

        slot.SetItem(ItemDB.Inst.Items[name].Info, itemCosts);
    }
}

public class ItemCost
{
    public string name;
    public int price;

    public ItemCost(string name, int price)
    {
        this.name = name;
        this.price = price;
    }
}
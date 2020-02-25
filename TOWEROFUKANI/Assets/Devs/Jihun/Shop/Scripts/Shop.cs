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
        SellNewItem("Potion of Healing");
        SellNewItem("Machinegun");
        SellNewItem("Talisman of Protection");
        //SellNewItem("Rose of Love");


        int slotCnt = slotRoot.childCount;

        for(int i = 0; i < slotCnt; i++)
        {
            ShopSlot slot = slotRoot.GetChild(i).GetComponent<ShopSlot>();

            slots.Add(slot);
        }
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
    }
    public void OpenShop()
    {
        gameObject.SetActive(true);
    }

    // 판매 아이템 목록에 아이템 추가.
    public void SellNewItem(string name, params object[] itemCost)
    {
        ShopSlot slot = Instantiate<ShopSlot>(slotPrefab);

        slot.transform.SetParent(slotRoot);
        slot.transform.localScale = Vector3.one;

        slot.SetItem(ItemDB.Inst.Items[name].Info);
    }
}

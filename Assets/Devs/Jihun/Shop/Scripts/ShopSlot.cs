using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    #region Var:UI
    public Item item;
    public int level;

    public Image itemSpriteImage;
    public Text itemNameTxt;
    public Text itemDescTxt;
    #endregion

    #region Var:Item Costs
    public List<ItemCost> itemCosts = new List<ItemCost>();

    public Transform itemCostsRoot;
    [SerializeField] private ItemCostSlot itemCostPrefab;

    Vector3 shopItemSpawnPos;
    #endregion

    public void SetSpawnPos(Vector3 pos)
    {
        shopItemSpawnPos = pos;
    }

    /// <summary>
    /// 상점에 새로운 품목을 추가할 때 상점 슬롯 객체를 초기화 해주는 메소드.
    /// </summary>
    /// <param name="item">아이템 프리팹</param>
    /// <param name="itemCosts">가격</param>
    public void SetItem(Item item, int level = 0, params ItemCost[] itemCosts)
    {
        if (item == null) return;

        ItemCostSlot slot;

        this.item = item;
        this.level = level;

        // 아이템 가격 설정
        for(int i = 0; i < itemCosts.Length; i++)
        {
            this.itemCosts.Add(itemCosts[i]);

            slot = Instantiate<ItemCostSlot>(itemCostPrefab);
            slot.SetPrice(itemCosts[i]);
            slot.transform.SetParent(itemCostsRoot);
            slot.transform.localScale = Vector3.one;
        }
            

        itemSpriteImage.gameObject.SetActive(true);
        itemSpriteImage.sprite = item.Info.Icon;

        itemNameTxt.text = item.Info.ItemName;
        itemDescTxt.text = item.Info.ItemDesc;
    }

    public void BuyItem()
    {
        if (!CheckPrice()) return;

        // 금액 지불
        PayCosts();

        UpgradableItem temp = ItemDB.Inst.SpawnDroppedItem(item, shopItemSpawnPos).Item as UpgradableItem;
        temp.AddLevel(level);
    }

    bool CheckPrice()
    {
        for (int i = 0; i < itemCosts.Count; i++)
        {
            // 화폐 설정
            Item itemKind = itemCosts[i].item;
            int count = PlayerInventoryManager.inventory.GetItemCount(itemKind.Info.ItemName);

            if (itemCosts[i].price > count)
            {
                Debug.Log(itemKind + " : " + itemCosts[i].price.ToString() + "/" + count.ToString());
                return false;
            }
        }

        return true;
    }

    void PayCosts()
    {
        for(int i = 0; i < itemCosts.Count; i++)
        {

            string itemKind = itemCosts[i].item.Info.ItemName;
            int payed = 0;
            int price = itemCosts[i].price;

            //해당 화폐의 가지고 있는 모든 묶음을 가져옴
            Item[] thisItems = PlayerInventoryManager.inventory.GetItems(itemKind);

            //해당 화폐 금액 지불 루프
            for (int j = 0; j < thisItems.Length; j++)
            {

                // 바로 지불 가능 할 때
                if((price - payed) <= thisItems[j].Info.Count)
                {
                    PlayerInventoryManager.inventory.RemoveItem(thisItems[j], price - payed);
                    break;
                }

                payed += thisItems[j].Info.Count;
                // 바로 지불 못하면 그 칸에 있는 모든 아이템 지불
                PlayerInventoryManager.inventory.RemoveItem(thisItems[j], thisItems[j].Info.Count);
                Debug.Log(thisItems[j].Info.Count + ", " + payed + ", " + price);
            }
        }
    }
}

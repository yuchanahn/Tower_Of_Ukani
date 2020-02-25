using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    #region Var:UI
    public ItemInfo item;

    public Image itemSpriteImage;
    public Text itemNameTxt;
    public Text itemDescTxt;
    #endregion

    #region Var:Item Costs
    public List<ItemCost> itemCosts = new List<ItemCost>();

    public Transform itemCostsRoot;
    [SerializeField] private ItemCostSlot itemCostPrefab;
    #endregion

    public void SetItem(ItemInfo item, params ItemCost[] itemCosts)
    {
        if (item == null) return;
        

        this.item = item;


        // 아이템 가격 설정
        for(int i = 0; i < itemCosts.Length; i++)
        {
            this.itemCosts.Add(itemCosts[i]);

            ItemCostSlot slot = Instantiate<ItemCostSlot>(itemCostPrefab);
            slot.SetPrice(itemCosts[i]);
            slot.transform.SetParent(itemCostsRoot);
            slot.transform.localScale = Vector3.one;
        }
            

        itemSpriteImage.gameObject.SetActive(true);
        itemSpriteImage.sprite = item.Icon;

        itemNameTxt.text = item.ItemName;
        itemDescTxt.text = item.ItemDesc;
    }

    public void BuyItem()
    {
        if (!CheckPrice()) return;

        // 금액 지불
        PayCosts();

        ItemDB.Inst.SpawnDroppedItem(ShopManager.Inst.shopItemSpawnPoint.position, item.ItemName);
    }

    bool CheckPrice()
    {
        for (int i = 0; i < itemCosts.Count; i++)
        {
            // 화폐 설정
            string itemKind = itemCosts[i].name;
            int count = 0;

            // 해당 화폐 불러옴
            Item[] items = PlayerInventoryManager.inventory.GetItems(itemKind);

            for (int j = 0; j < items.Length; j++)
            {
                count += items[j].Info.Count;
            }

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

            string itemKind = itemCosts[i].name;
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

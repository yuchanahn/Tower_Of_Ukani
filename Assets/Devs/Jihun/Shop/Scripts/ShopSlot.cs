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

        UpgradableItem temp = ItemDB.Inst.SpawnDroppedItem(item, shopItemSpawnPos, spawnMode:ItemDB.SpawnMode.Clone).Item as UpgradableItem;
    }

    bool CheckPrice()
    {
        for (int i = 0; i < itemCosts.Count; i++)
        {
            // 화폐 설정
            Item itemKind = itemCosts[i].item;
            int count = PlayerInventoryManager.inventory.GetItemCount(itemKind.Info.ItemName);

            if (itemCosts[i].count > count)
            {
                Debug.Log(itemKind + " : " + itemCosts[i].count.ToString() + "/" + count.ToString());
                return false;
            }
        }

        return true;
    }
    Item GetItem<T>() where T : Item
    {
        return ItemDB.Inst.GetItemPrefab<T>();
    }

    void PayCosts()
    {
        for(int i = 0; i < itemCosts.Count; i++)
        {
            PlayerInventoryManager.inventory.RemoveItem(itemCosts[i].item.GetType(), itemCosts[i].count);
        }
    }
}

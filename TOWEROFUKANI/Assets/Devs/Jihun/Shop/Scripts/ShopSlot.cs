using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public ItemInfo item;

    public Image itemSpriteImage;
    public Text itemNameTxt;
    public Text itemDescTxt;

    

    public void SetItem(ItemInfo item)
    {
        this.item = item;

        if (item == null) return;

        itemSpriteImage.gameObject.SetActive(true);
        itemSpriteImage.sprite = item.Icon;

        itemNameTxt.text = item.ItemName;
        itemDescTxt.text = item.ItemDesc;
    }

    public void BuyItem()
    {
        int curGold = 0;

        //가격 체크
        Item[] gold = PlayerInventoryManager.inventory.GetItems("Gold");

        if (gold != null)
        {
            for (int i = 0; i < gold.Length; i++)
            {
                curGold += gold[i].Info.Count;
            }

            Debug.Log(curGold + "원");
        }

        ItemDB.Inst.SpawnDroppedItem(item.ItemName);
    }
}

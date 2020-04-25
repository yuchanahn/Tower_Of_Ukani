using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCostSlot : MonoBehaviour
{
    public Image itemSpriteImage;
    public Text itemPriceTxt;

    ItemCost itemCost;


    public void SetPrice(ItemCost itemCost)
    {
        itemSpriteImage.sprite = itemCost.item.Info.Icon;

        this.itemCost = itemCost;
    }

    private void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        string str = (itemCost.count.ToString() + " / " + GetItemCount(itemCost.item.Info.ItemName).ToString());
        itemPriceTxt.text = str;
    }

    int GetItemCount(string name)
    {
        Item[] items = PlayerInventoryManager.inventory.GetItems(name);
        int count = 0;
        for(int i = 0; i < items.Length;i++)
        {
            count += items[i].Info.Count;
        }

        return count;
    }
}

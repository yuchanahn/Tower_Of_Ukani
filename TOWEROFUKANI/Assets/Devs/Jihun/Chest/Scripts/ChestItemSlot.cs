using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestItemSlot : MonoBehaviour
{
    public ChestItem item;

    Chest chest;

    public Image itemSpriteImage;
    public Text itemNameTxt;
    public Text itemDescTxt;
    
    public void SetItem(ChestItem item, Chest chest)
    {
        ItemInfo info = item.info;

        if (info == null) return;


        this.item = item;
        this.chest = chest;


        itemSpriteImage.gameObject.SetActive(true);
        itemSpriteImage.sprite = info.Icon;

        //아이템의 이름과 갯수
        string nameStr = info.ItemName;
        if(item.count > 1)
            nameStr += (" x" + item.count.ToString());
        itemNameTxt.text = nameStr;
        itemDescTxt.text = info.ItemDesc;
    }

    public void SelectThis()
    {
        chest.selectedItem = item;
    }
}

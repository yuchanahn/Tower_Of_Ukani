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
        if (item == null) return;

        //넣을 아이템의 정보
        ItemInfo info = item.item.Info;

        this.item = item;
        this.chest = chest;


        itemSpriteImage.gameObject.SetActive(true);
        itemSpriteImage.sprite = info.Icon;

        //아이템의 이름 칸에 넣을 문자열
        string nameStr = info.ItemName;
        if(item.count > 1)
            nameStr += (" x" + item.count.ToString());
        itemNameTxt.text = nameStr;
        //===========================
        itemDescTxt.text = info.ItemDesc;
    }

    //슬롯 클릭했을때 선택
    public void SelectThis()
    {
        chest.SelectItem(this);
    }
}
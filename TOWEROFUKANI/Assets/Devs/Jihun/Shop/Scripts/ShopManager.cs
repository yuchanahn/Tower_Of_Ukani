using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : SingletonBase<ShopManager>
{
    public Shop shopUI;
    public Transform shopItemSpawnPoint;

    private void Start()
    {
        shopUI = Resources.FindObjectsOfTypeAll<Shop>()[0];
        shopItemSpawnPoint = GameObject.Find("ShopItemSpawnPoint").transform;
    }


    public void CloseShop()
    {
        shopUI.gameObject.SetActive(false);
    }
    public void OpenShop()
    {
        shopUI.gameObject.SetActive(true);
    }

    public void ToggleShop()
    {
        if (shopUI.gameObject.activeSelf)
            CloseShop();
        else
            OpenShop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleShop();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : SingletonBase<ShopManager>
{
    public Shop shopUI;
    public Transform shopItemSpawnPoint;

    public float shopRange;

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
        //거리가 너무 멀면 실행 안함.
        if (!DistanceCheck()) return;

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
        if (Input.GetKeyDown(KeyCode.Z))
            ToggleShop();

        //거리가 너무 멀면 실행 안함.
        if (!DistanceCheck()) CloseShop();
    }

    bool DistanceCheck()
    {
        float distance = Vector3.Distance(GM.PlayerPos, shopItemSpawnPoint.position);

        Debug.Log(distance.ToString() + ", " + shopRange.ToString());
        return (distance < shopRange);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestManager : SingletonBase<ChestManager>
{
    List<InteractiveObj> interactiveObjs = new List<InteractiveObj>();

    public InteractiveObj curObj = null;

    Transform player;

    public float chestRange;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        player = GM.Player.transform;
        ListInit();
    }

    /// <summary>
    /// 상자들을 관리할 리스트에 씬에있는 상자들을 넣어줌
    /// </summary>
    void ListInit()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Interactive");
        InteractiveObj interObj;
        for(int i = 0;i < objs.Length; i++)
        {
            interObj = objs[i].GetComponent<InteractiveObj>();
            if (interObj)
                interactiveObjs.Add(interObj);
        }
    }

    bool IsCloseToPlayer(InteractiveObj chest)
    {
        return (Vector2.Distance(player.position, chest.transform.position) < chestRange);
    }

    // Update is called once per frame
    void Update()
    {
        //열려있는 상자와 플레이어 간의 거리 확인
        CheckOpenedChestPos();

        //매 프레임 입력 받고 상자 여는지 확인
        GetInput();
    }

    void CheckOpenedChestPos()
    {
        if (curObj == null) return;

        if(!IsCloseToPlayer(curObj))
        {
            curObj = curObj.Interact();
        }
    }

    void GetInput()
    {
        //입력 받았을 때
        if (!Input.GetKeyDown(PlayerActionKeys.Interact)) return;

        //이미 열려있는 상자가 있으면
        if(curObj)
        {
            //그 상자 닫고 함수 종료
            curObj = curObj.Interact();
            return;
        }

        //열려있지 않은 상자들을 거리순으로 정렬
        var chestsByDist =
            from chest in interactiveObjs
            where Vector2.Distance(chest.transform.position, player.position) < chestRange
            where chest.isInteractive
            orderby Vector2.Distance(chest.transform.position, player.position)
            select chest;
        //가장 가까운 상자가
        InteractiveObj closest = chestsByDist.DefaultIfEmpty().First();
        
        //존재하면
        if (closest != null)
        {
            if (curObj)
                Debug.Log(curObj.name);
            //열린상자 갱신하고 엶
            curObj = closest.Interact();
        }
    }

    public void StopInteract()
    {
        curObj = null;
    }
}

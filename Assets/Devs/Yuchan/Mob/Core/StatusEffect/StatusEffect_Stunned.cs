using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Stunned : StatusEffect_Base
{
    GameObject StunnedObj;

    protected override void Start()
    {
        base.Start();
    }

    public static void Create(GameObject obj, GameObject StunnedObj, float StunnedTime)
    {
        var SE_Obj = obj.GetComponent<StatusEffect_Object>();
        if (SE_Obj is null) return;

        StatusEffect_Stunned SE_Stunned = obj.GetComponent<StatusEffect_Stunned>();
        // 각 상태이상에 대한 초기화 설정을 해준다.
        if (SE_Stunned is null)
            SE_Stunned      = obj.AddComponent<StatusEffect_Stunned>();




        // 우선 순위 설정 높을수록 우선순위가 높음.
        SE_Stunned.priority = 10;
        SE_Stunned.StunnedObj = StunnedObj;
        StunnedObj.SetActive(true);

        Destroy(SE_Stunned, StunnedTime);
    }
    void Update()
    {
        var SE_Stunned  =    GetComponent<StatusEffect_Stunned>();
        var SE_Obj      =    GetComponent<StatusEffect_Object>();

        SE_Stunned.SetValue(SE_Obj.StatusEffect_Ani, eMobAniST.Stunned);
        SE_Stunned.SetValue(SE_Obj.StatusEffect_SpeedMult, 0);
        SE_Stunned.SetValue(SE_Obj.StatusEffect_AttackAble, false);
        SE_Stunned.SetValue(SE_Obj.StatusEffect_FollowAble, false);
        SE_Stunned.SetValue(SE_Obj.StatusEffect_ChangeDirAble, false);
        SE_Stunned.SetValue(SE_Obj.StatusEffect_NoTask, true);
    }

    protected override void OnSEDestroy()
    {
        var SE_Stunned  = GetComponent<StatusEffect_Stunned>();
        var SE_Obj      = GetComponent<StatusEffect_Object>();

        SE_Stunned.SetValue(SE_Obj.StatusEffect_Ani, eMobAniST.Last);
        SE_Stunned.SetValue(SE_Obj.StatusEffect_SpeedMult, 1f);
        SE_Stunned.SetValue(SE_Obj.StatusEffect_AttackAble, true);
        SE_Stunned.SetValue(SE_Obj.StatusEffect_ChangeDirAble, true);
        SE_Stunned.SetValue(SE_Obj.StatusEffect_FollowAble, true);
        SE_Stunned.SetValue(SE_Obj.StatusEffect_NoTask, false);

        StunnedObj.SetActive(false);
    }
}

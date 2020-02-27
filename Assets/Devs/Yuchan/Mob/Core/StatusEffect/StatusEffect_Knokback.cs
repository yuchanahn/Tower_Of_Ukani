using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Knokback : StatusEffect_Base
{
    protected override void Start()
    {
        base.Start();
    }

    public static void Create(GameObject obj)
    {
        var SE_Obj = obj.GetComponent<StatusEffect_Object>();
        if (SE_Obj is null) return;

        StatusEffect_Knokback knokback = obj.GetComponent<StatusEffect_Knokback>();
        // 각 상태이상에 대한 초기화 설정을 해준다.
        if (knokback is null)
            knokback = obj.AddComponent<StatusEffect_Knokback>();
        
        // 모든 옵션에 관한, 디폴트 우선 순위 설정, 높을수록 우선순위가 높음.
        knokback.priority = 9;
    }
    void Update()
    {
        //넉백을 수행 시킬 코드블럭.
        
        SetValue(SE_Obj.StatusEffectSpeedMult, 0);
        SetValue(SE_Obj.StatusEffectAttackAble, false);
        SetValue(SE_Obj.StatusEffectFollowAble, false);
        SetValue(SE_Obj.StatusEffectNoTask, true);
    }

    protected override void OnSEDestroy()
    {
        SetValue(SE_Obj.StatusEffectSpeedMult, 1f);
        SetValue(SE_Obj.StatusEffectAttackAble, true);
        SetValue(SE_Obj.StatusEffectFollowAble, true);
        SetValue(SE_Obj.StatusEffectNoTask, false);
    }
}

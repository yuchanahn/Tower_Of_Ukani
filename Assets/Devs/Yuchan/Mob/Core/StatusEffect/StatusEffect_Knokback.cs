using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Knokback : StatusEffect_Base
{
    public static void CreateNormal(GameObject o, Vector2 d) => Create(o, d, 3, 0.5f);
    public static void CreateSuper(GameObject o, Vector2 d) => Create(o, d, 4, 0.5f);

    public static void Create(GameObject obj, Vector2 dir, float power, float tick)
    {
        var SE_Obj = obj.GetComponent<StatusEffect_Object>();
        if (SE_Obj is null) return;

        StatusEffect_Knokback knokback = obj.GetComponent<StatusEffect_Knokback>();
        // 각 상태이상에 대한 초기화 설정을 해준다.
        if (knokback is null)
            knokback = obj.AddComponent<StatusEffect_Knokback>();
        

        // 모든 옵션에 관한, 디폴트 우선 순위 설정, 높을수록 우선순위가 높음.
        knokback.priority = 9;
        knokback.mDir2d = dir;
        knokback.pow = power;

        Destroy(knokback, tick);
    }

    Vector2 mDir2d;
    float pow;

    void Update()
    {
        //넉백을 수행 시킬 코드블럭.
        
        SetValue(SE_Obj.StatusEffect_SpeedMult, pow, 11);
        SetValue(SE_Obj.StatusEffect_Dir2d, mDir2d);
        SetValue(SE_Obj.StatusEffect_NoTask, true);
    }

    protected override void OnSEDestroy()
    {
        SetValue(SE_Obj.StatusEffect_SpeedMult, 1f, 11);
        SetValue(SE_Obj.StatusEffect_Dir2d, Vector2.zero);
        SetValue(SE_Obj.StatusEffect_NoTask, false);
    }
}

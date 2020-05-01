using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Knokback : StatusEffect_Base
{
    //public static void CreateNormal(GameObject o, Vector2 d) => Create(o, d, 3, 0.5f);
    //public static void CreateSuper(GameObject o, Vector2 d) => Create(o, d, 4, 0.5f);

    public static void Create(GameObject obj, Vector2 dir, AnimationCurve power)
    {
        var SE_Obj = obj.GetComponent<StatusEffect_Object>();
        if (SE_Obj is null) return;

        StatusEffect_Knokback knokback = obj.GetComponent<StatusEffect_Knokback>();

        if (knokback.IsNull())
        {
            knokback = obj.AddComponent<StatusEffect_Knokback>();
            knokback.destroy_timer = new YCTimerData(power.keys[power.length - 1].time, () => Destroy(knokback));
        }

        knokback.priority = 9;
        knokback.mDir2d = dir;
        knokback.pow = power;
        
        YCTimer.Add(knokback.destroy_timer);
    }

    Vector2 mDir2d;
    AnimationCurve pow;

    void Update()
    {
        //넉백을 수행 시킬 코드블럭.

        SetValue(SE_Obj.StatusEffect_Ani, eMobAniST.Stunned);
        SetValue(SE_Obj.StatusEffect_Dir2d, mDir2d);
        SetValue(SE_Obj.StatusEffect_NoTask, true);
        SetValue(SE_Obj.StatusEffect_UseSEVelCurve, true);
        SetValue(SE_Obj.StatusEffect_VelCurve, pow);
    }

    protected override void OnSEDestroy()
    {
        SetValue(SE_Obj.StatusEffect_Ani, eMobAniST.Last);
        SetValue(SE_Obj.StatusEffect_Dir2d, Vector2.zero);
        SetValue(SE_Obj.StatusEffect_NoTask, false);
        SetValue(SE_Obj.StatusEffect_UseSEVelCurve, false);
        SetValue(SE_Obj.StatusEffect_VelCurve, new AnimationCurve());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Slow : StatusEffect_Base
{
    float pow;
    public static void CreateTest(GameObject o) => Create(o, 0.9f, 1f);
    // power는 1이 최댓값.  0 = 0%, 1 = 100%.
    public static void Create(GameObject obj, float power, float time)
    {
        var SE_Obj = obj.GetComponent<StatusEffect_Object>();
        if (SE_Obj is null) return;

        power = Mathf.Min(power, 1f);
        StatusEffect_Slow slow = obj.GetComponent<StatusEffect_Slow>();

        // 각 상태이상에 대한 초기화 설정을 해준다.
        if (slow.IsNotNull() && slow.pow > power) return;
        else if(slow.IsNull())
        {
            slow = obj.AddComponent<StatusEffect_Slow>();
            slow.destroy_timer = new YCTimerData(time, () => Destroy(slow));
        }

        // 모든 옵션에 관한, 디폴트 우선 순위 설정, 높을수록 우선순위가 높음.
        slow.priority = 8;
        slow.pow = power;
        YCTimer.Add(slow.destroy_timer);
    }

    void Update()
    {
        SetValue(SE_Obj.StatusEffect_SpeedMult, 1 - pow);
    }

    protected override void OnSEDestroy()
    {
        SetValue(SE_Obj.StatusEffect_SpeedMult, 1f);
    }
}
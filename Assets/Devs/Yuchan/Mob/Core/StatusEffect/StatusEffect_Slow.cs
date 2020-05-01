using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class SlowData
{
    public float power;
    public float time;
    public float t; //경과시간.
}

public class StatusEffect_Slow : StatusEffect_Base
{
    float pow;

    List<SlowData> slows = new List<SlowData>();

    public static void CreateTest(GameObject o) => Create(o, 0.9f, 1f);
    // power는 1이 최댓값.  0 = 0%, 1 = 100%.
    public static void Create(GameObject obj, float power, float time)
    {
        var SE_Obj = obj.GetComponent<StatusEffect_Object>();
        if (SE_Obj is null) return;

        SlowData d = new SlowData() { power = power, time = time, t = 0 };
        power = Mathf.Min(power, 1f);
        StatusEffect_Slow slow = obj.GetComponent<StatusEffect_Slow>();

        if (slow.IsNull())
        {
            slow = obj.AddComponent<StatusEffect_Slow>();
        }
        slow.slows.Add(d);

        // 모든 옵션에 관한, 디폴트 우선 순위 설정, 높을수록 우선순위가 높음.
        slow.priority = 8;
        slow.pow = power;
    }

    void Update()
    {
        var v = slows.Select(x => x.power).ToList();
        v.Sort();

        SetValue(SE_Obj.StatusEffect_SpeedMult, 1 - v.First());

        List<SlowData> pop_datas = new List<SlowData>();
        slows.ForEach(x => (x.time <= (x.t += Time.deltaTime)).IF(() =>
        {
            pop_datas.Add(x);
        }));
        pop_datas.ForEach(x=>slows.Remove(x));
        if (slows.Count == 0) Destroy(this);
    }

    protected override void OnSEDestroy()
    {
        SetValue(SE_Obj.StatusEffect_SpeedMult, 1f);
    }
}
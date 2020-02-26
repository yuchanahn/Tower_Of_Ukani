using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_IgnoreHit: StatusEffect_Base
{
    protected override void Start()
    {
        base.Start();
    }


    public static void Create(GameObject obj)
    {
        var SE_Obj = obj.GetComponent<StatusEffect_Object>();
        if (SE_Obj is null) return;

        StatusEffect_IgnoreHit IgnoreHit = obj.GetComponent<StatusEffect_IgnoreHit>();

        // 각 상태이상에 대한 초기화 설정을 해준다.
        if (IgnoreHit is null)
            IgnoreHit = obj.AddComponent<StatusEffect_IgnoreHit>();

        // 우선 순위 설정 높을수록 우선순위가 높음.
        IgnoreHit.priority = 0;
    }

    void Update()
    {
    }

    public void Destroy()
    {
        OnSEDestroy(); 
        Destroy(this);
    }

    protected override void OnSEDestroy()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Stunned : StatusEffect_Base
{
    float StunnedT = 0;
    GameObject StunnedObj;

    protected override void Start()
    {
        base.Start();
    }
    void Init()
    {
        ATimer.SetAndReset(gameObject.name + GetInstanceID(), StunnedT, ()=> { OnSEDestroy(); Destroy(this);  });
    }
    void OnDestroy()
    {
        ATimer.Pop(gameObject.name + GetInstanceID());
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
        SE_Stunned.StunnedT = StunnedTime;
        SE_Stunned.Init();
        SE_Stunned.StunnedObj = StunnedObj;
        StunnedObj.SetActive(true);
    }
    void Update()
    {
        var SE_Stunned  =    GetComponent<StatusEffect_Stunned>();
        var SE_Obj      =    GetComponent<StatusEffect_Object>();

        SE_Stunned.SetValue<eMobAniST>(SE_Obj.StatusEffectAni, eMobAniST.Stunned);
        SE_Stunned.SetValue<float>(SE_Obj.StatusEffectSpeedMult, 0);
        SE_Stunned.SetValue<bool>(SE_Obj.StatusEffectAttackAble, false);
        SE_Stunned.SetValue<bool>(SE_Obj.StatusEffectFollowAble, false);
        SE_Stunned.SetValue<bool>(SE_Obj.StatusEffectChangeDirAble, false);
    }

    protected override void OnSEDestroy()
    {
        var SE_Stunned  = GetComponent<StatusEffect_Stunned>();
        var SE_Obj      = GetComponent<StatusEffect_Object>();

        SE_Stunned.SetValue<eMobAniST>  (SE_Obj.StatusEffectAni, eMobAniST.Last);
        SE_Stunned.SetValue<float>      (SE_Obj.StatusEffectSpeedMult, 1f);
        SE_Stunned.SetValue<bool>       (SE_Obj.StatusEffectAttackAble, true);
        SE_Stunned.SetValue<bool>       (SE_Obj.StatusEffectChangeDirAble, true);
        SE_Stunned.SetValue<bool>       (SE_Obj.StatusEffectFollowAble, true);

        StunnedObj.SetActive(false);
    }
}

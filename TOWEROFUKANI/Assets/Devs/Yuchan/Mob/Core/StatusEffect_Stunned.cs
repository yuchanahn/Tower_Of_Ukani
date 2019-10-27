using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Stunned : StatusEffect_Base
{
    float StunnedT = 0;

    protected override void Start()
    {
        base.Start();
        Destroy(this, StunnedT);
    }

    static void Create(GameObject obj, float StunnedTime)
    {
        var st = obj.AddComponent<StatusEffect_Stunned>();

        /*
         * 각 상태이상에 대한 초기화 설정을 해준다.
         * 
         * 
         * 
         * 
         * 
         * 
         */

        // 우선 순위 설정 높을수록 우선순위가 높음.
        st.priority = 10; 

        st.StunnedT = StunnedTime;
        st.STEFData.HurtStop = 1;
        st.STEFData.MoveStop = 1;
        st.STEFData.AttackStop = 1;
        st.STEFData.UseStatusEffect = 1;
        st.STEFData.StatusEffect = eMobAniST.Stunned;
    }
}

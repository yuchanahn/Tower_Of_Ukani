using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MobHPBar : Object_ObjectPool<MobHPBar>
{
    [SerializeField] AUI_Slider mHpBar;
    [SerializeField] AStat mStat;
    [SerializeField] Transform mTarget;
    [HideInInspector] public float sizeY;

    // ===============================================================
    //        ## 생성 소멸자
    // ===============================================================

    public override void Begin()
    {
    }
    public override void End()
    {
        if (mTarget) mTarget.GetComponent<AStat>().mHPBarUI = null;
    }

    // ===============================================================

    private void Update()
    {
        HpBarUpdate();
        if (mTarget == null)
        {
            DestroyObj();
        }
    }

    // ===============================================================
    //      ## 크리에이터
    // ===============================================================

    public static MobHPBar Create(AStat target, float destroyT)
    {
        var o = CreateUI(target.transform.position).GetComponent<MobHPBar>();
        o.Init(target);
        o.SetDestroyTimer(destroyT);
        return o;
    }

    // ===============================================================


    public void SetDestroyTimer(float destroyT)
    {
        DestroyObj(destroyT);
    }

    public void Init(AStat stat)
    {
        mStat = stat;
        mTarget = stat.transform;

        HpBarUpdate();
    }

    void HpBarUpdate()
    {
        if (mTarget)
        {
            (transform as RectTransform).position = Camera.main.WorldToScreenPoint(mTarget.position + Vector3.up * ((sizeY / 2) + 1f));

            mHpBar.CUR_ = mStat.HP;
            mHpBar.MAX_ = mStat.MAXHP;
        }
    }
}

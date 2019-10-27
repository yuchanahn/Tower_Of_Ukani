using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Base : MonoBehaviour
{
    [SerializeField] protected int priority;
    [SerializeField] protected int Last_priority;
    [SerializeField] protected StatusEffectData STEFData;

    Mob_Base mob;
    protected virtual void Start()
    {
        mob = GetComponent<Mob_Base>();
        if (mob.m_STEFData.priority < priority)
        {
            mob.m_STEFData.StatusEffect = STEFData.StatusEffect;
            Last_priority = mob.m_STEFData.priority;
            mob.m_STEFData.priority = priority;
        }

        mob.m_STEFData.HurtStop += STEFData.HurtStop;
        mob.m_STEFData.MoveStop += STEFData.MoveStop;
        mob.m_STEFData.AttackStop += STEFData.AttackStop;
        mob.m_STEFData.UseStatusEffect += STEFData.UseStatusEffect;
    }
    protected virtual void OnDestroy()
    {
        mob.m_STEFData.HurtStop -= STEFData.HurtStop;
        mob.m_STEFData.MoveStop -= STEFData.MoveStop;
        mob.m_STEFData.AttackStop -= STEFData.AttackStop;
        mob.m_STEFData.UseStatusEffect -= STEFData.UseStatusEffect;
        /// 꼭 고칠것....
        mob.m_STEFData.priority = Last_priority; // 근데 그 전에 Last 가  끝날 수 있음
    }
}

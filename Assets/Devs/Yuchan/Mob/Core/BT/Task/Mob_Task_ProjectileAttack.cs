using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mob_Task_ProjectileAttack : MonoBehaviour, ITask
{
    [SerializeField] GameObject Projectile;
    [SerializeField] float CoolTime;
    protected float coolTimeT;

    public Transform Target { get; set; } = null;


    protected virtual void Awake()
    {
        coolTimeT = CoolTime;
    }

    virtual protected void Mob_Task_ProjectileAttack_AttackStart_AniEvent()
    {
        Mob_Task_ProjectileAttack_Attack();
    }
    void Mob_Task_ProjectileAttack_AttackEnd_AniEvent()
    {
        OnAttackEnd();
    }

    protected virtual GameObject Mob_Task_ProjectileAttack_Attack()
    {
        var o = Projectile.GetComponent<IObjectPool>().CreateThis(transform.position);
        o.GetComponent<IObjectPool>().Init((GM.PlayerPos - transform.position).normalized);
        return o;
    }
    protected virtual void OnAttackEnd()
    {
    }


    protected virtual void OnAttackAble()
    {
        Mob_Task_ProjectileAttack_Attack();
    }

    public void AddAttackCoolTimeT()
    {
        coolTimeT.AddDT();
    }

    public virtual bool Tick()
    {
        if (Target is null)
        {
            return false;
        }

        if (coolTimeT >= CoolTime)
        {
            coolTimeT = 0;
            OnAttackAble();
        }

        return true;
    }
}

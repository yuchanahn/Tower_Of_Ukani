using System;
using System.Collections.Generic;
using UnityEngine;

public enum eMobAniST
{
    Attack_Pre,
    Attack_Post,
    Hit,
    Walk,
    Idle,
    AirborneDown,
    AirborneUp,
    Stunned,
    Fly,
    PreCry,
    Cry,
    Spread,
    Merge,
    Death,

    Last,
}

[Serializable]
public struct AniSpeedData
{
    public eMobAniST ST;
    public float t;
}
public enum eAttackST
{
    PreAttack,
    Attack
}


public abstract class Mob_Base : MonoBehaviour, IHurt
{
    protected const float OverlapSlow = 0.65f;
    public readonly StatusID StatusID = new StatusID();
    [SerializeField] LayerMask CreatureLayer;
    [SerializeField] protected Vector2 MobSize;


    [SerializeField] 
    protected AniSpeedData[] mAniSpeedData;
    protected Dictionary<eAttackST, Action> mAttack = new Dictionary<eAttackST, Action>();
    protected Dictionary<eMobAniST, (string, float)> m_Ani = new Dictionary<eMobAniST, (string, float)>();

    protected bool CheckOverlapSlow(Vector2 size, Vector2 dir)
    {
        var o = Physics2D.OverlapBoxAll(transform.position, size, 0, CreatureLayer);
        bool over = false;
        var c = GetComponent<Collider2D>();
        foreach (var i in o)
        {
            if (i == c) continue;
            if (i.gameObject.CompareTag("Player")) continue;
            if (Vector2.Angle(i.transform.position - transform.position, dir) < 30f)
            {
                over = true;
            }
        }
        return over;
    }

    protected void Init_AniDuration()
    {
        for (int i = 0; i < (int)eMobAniST.Last; i++)
            m_Ani[(eMobAniST)i] = (((eMobAniST)i).ToString(), 0);
        foreach (var i in mAniSpeedData)
            m_Ani[i.ST] = (i.ST.ToString(), i.t);
    }

    public abstract void OnSuccessfulAttack();
    public abstract void OnDead();
    public abstract void OnHurt();
}

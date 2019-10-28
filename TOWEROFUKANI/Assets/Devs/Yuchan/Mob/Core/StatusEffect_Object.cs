using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_Stat<T>
{
    private T value = default;
    protected int max_priority = 0;
    public T Value => value;

    public SE_Stat(T data)
    {
        value = data;
    }

    public void SetValue(int priority, T data)
    {
        if (priority >= max_priority)
        {
            max_priority = priority;
            value = data;
        }
    }
}

public class StatusEffect_Object : MonoBehaviour
{
    public bool Use;

    List<StatusEffect_Base> statusEffect_s;

    private SE_Stat<float>      statusEffectSpeed = new SE_Stat<float>(1f);
    private SE_Stat<bool>       statusEffectFallowAble = new SE_Stat<bool>(true);
    private SE_Stat<bool>       statusEffectAttackAble = new SE_Stat<bool>(true);
    private SE_Stat<bool>       statusEffectChangeDirAble = new SE_Stat<bool>(true);
    private SE_Stat<eMobAniST>  statusEffectAni = new SE_Stat<eMobAniST>(eMobAniST.Last);

    public SE_Stat<float> StatusEffectSpeedMult => statusEffectSpeed;
    public SE_Stat<eMobAniST> StatusEffectAni => statusEffectAni;
    public SE_Stat<bool> StatusEffectFollowAble => statusEffectFallowAble;
    public SE_Stat<bool> StatusEffectAttackAble => statusEffectAttackAble;
    public SE_Stat<bool> StatusEffectChangeDirAble => statusEffectChangeDirAble;


    public bool SEAttackAble    => Use ? statusEffectAttackAble.Value : true;
    public bool SEFallowAble    => Use ? statusEffectFallowAble.Value : true;
    public bool SEChangeDirAble => Use ? statusEffectChangeDirAble.Value : true;
    public float SESpeedMult    => Use ? statusEffectSpeed .Value : 1f;
    public eMobAniST SEAni      => Use ? statusEffectAni.Value : eMobAniST.Last;

    public void Remove(StatusEffect_Base SEBase)
    {
        statusEffect_s.Remove(SEBase);
    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(10,10,100,20), "Create Stunnd!"))
        {
            StatusEffect_Stunned.Create(gameObject, 10f);
        }
    }
}
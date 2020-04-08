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
    [SerializeField] public GameObject StunnedObj;

    List<StatusEffect_Base> statusEffect_s = new List<StatusEffect_Base>();

    private SE_Stat<float>      statusEffectSpeed = new SE_Stat<float>(1f);
    private SE_Stat<Vector2>    statusEffectDir2d = new SE_Stat<Vector2>(Vector2.zero);
    private SE_Stat<AnimationCurve> statusEffectVelCurve = new SE_Stat<AnimationCurve>(new AnimationCurve());
    private SE_Stat<bool>       statusEffectFallowAble = new SE_Stat<bool>(true);
    private SE_Stat<bool>       statusEffectAttackAble = new SE_Stat<bool>(true);
    private SE_Stat<bool>       statusEffectChangeDirAble = new SE_Stat<bool>(true);
    private SE_Stat<bool>       statusEffectNoTask = new SE_Stat<bool>(false);
    private SE_Stat<eMobAniST>  statusEffectAni = new SE_Stat<eMobAniST>(eMobAniST.Last);
    private SE_Stat<bool>       statusEffectUseSEVel = new SE_Stat<bool>(false);
    private SE_Stat<bool>       statusEffectUseSEVelCurve = new SE_Stat<bool>(false);

    public SE_Stat<bool> StatusEffect_NoTask => statusEffectNoTask;
    public SE_Stat<float> StatusEffect_SpeedMult => statusEffectSpeed;
    public SE_Stat<Vector2> StatusEffect_Dir2d => statusEffectDir2d;
    public SE_Stat<AnimationCurve> StatusEffect_VelCurve => statusEffectVelCurve;
    public SE_Stat<eMobAniST> StatusEffect_Ani => statusEffectAni;
    public SE_Stat<bool> StatusEffect_FollowAble => statusEffectFallowAble;
    public SE_Stat<bool> StatusEffect_AttackAble => statusEffectAttackAble;
    public SE_Stat<bool> StatusEffect_ChangeDirAble => statusEffectChangeDirAble;
    public SE_Stat<bool> StatusEffect_UseSEVel => statusEffectUseSEVel;
    public SE_Stat<bool> StatusEffect_UseSEVelCurve => statusEffectUseSEVelCurve;

    public bool NoTask        => Use ? StatusEffect_NoTask.Value : false;
    public bool AttackAble    => Use ? statusEffectAttackAble.Value : true;
    public bool FallowAble    => Use ? statusEffectFallowAble.Value : true;
    public bool ChangeDirAble => Use ? statusEffectChangeDirAble.Value : true;
    public bool UseSEVel      => Use ? statusEffectUseSEVel.Value : false;
    public bool UseSEVelCurve => Use ? statusEffectUseSEVelCurve.Value : false;
    public float SpeedMult    => Use ? statusEffectSpeed .Value : 1f;
    public Vector2 EffectDir2d  => Use ? statusEffectDir2d.Value : Vector2.zero;
    public AnimationCurve VelCurve => Use ? StatusEffect_VelCurve.Value : new AnimationCurve();
    public eMobAniST Ani      => Use ? statusEffectAni.Value : eMobAniST.Last;

    public void Remove(StatusEffect_Base SEBase)
    {
        statusEffect_s.Remove(SEBase);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //StatusEffect_Stunned.Create(gameObject, StunnedObj, 3f);
            StatusEffect_Knokback.Create(gameObject, Vector2.up, GM.testcur);
        }
    }
}
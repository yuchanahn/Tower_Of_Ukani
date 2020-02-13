﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AStat : MonoBehaviour, IDamage
{
    [SerializeField] private float mHP;
    [SerializeField] private float mDamage;

    private float mMAXHP;
    private float mDmg = 0;


    MobDamageText Dmg = null;
    public MobHPBar mHPBarUI = null;
    bool IsIgnoreHit => GetComponent<StatusEffect_IgnoreHit>();
    public float HP { get { return mHP; } set { mHP = value; if(mHP <= 0) GetComponent<IHurt>().OnDead(); } }

    public float MAXHP { get => mMAXHP; }
    public float Damage => mDamage;

    private void Awake()
    {
        mMAXHP = mHP;
    }

    public float Hit(AttackData atkData)
    {
        if (IsIgnoreHit) return 0;
        var dmg = atkData.damage.Value;
        HP -= dmg;
        mDmg += dmg;

        var id = GetComponent<Mob_Base>().StatusID;

        //PlayerStatus.Inst.Trigger(GetComponent<Mob_Base>(), MobAction.Hit);

        GetComponent<HitColorEffect>().OnHit();
        GetComponent<IHurt>().OnHurt();

        ATimer.SetAndReset(gameObject.name + GetInstanceID().ToString(), 0.1f,
        () =>
        {
            Dmg = null; mDmg = 0;
        });
        if (Dmg == null) Dmg = MobDamageText.Show(MathD.Round(mDmg), transform.position);
        Dmg.mText.text = MathD.Round(mDmg).ToString();
        Dmg.SetPoint(transform.position);
        Dmg.Begin();


        if (mHPBarUI is null)
        {
            mHPBarUI = MobHPBar.Create(this, 3f);
        }
        else
        {
            mHPBarUI.SetDestroyTimer(3f);
        }

        return mHP;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AStat : MonoBehaviour, IDamage
{
    [SerializeField] private float mHP;
    [SerializeField] private float mDamage;

    private float mMAXHP;
    private float mDmg = 0;


    MobDamageText mDmgTextUI = null;
    public MobHPBar mHPBarUI = null;
    bool IsIgnoreHit => GetComponent<StatusEffect_IgnoreHit>();
    public float HP { get { return mHP; } set { mHP = value; if(mHP <= 0) GetComponent<IHurt>().OnDead(); } }
    public int DisplayDmg => mDmg == 0 ? 0 : Mathf.Max((int)mDmg, 1);
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
        
        //StatusEffect_Knokback.CreateNormal(gameObject, -GetComponent<Mob_Base>().DirToPlayer);

        ATimer.SetAndReset(gameObject.name + GetInstanceID().ToString(), 0.1f,
        () =>
        {
            mDmgTextUI = null; mDmg = 0;
        });
        if (mDmgTextUI == null) mDmgTextUI = MobDamageText.Show(DisplayDmg, transform.position);
        mDmgTextUI.mText.text = DisplayDmg.ToString();
        mDmgTextUI.SetPoint(transform.position);
        mDmgTextUI.Begin();


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

    public bool CheckDeath(AttackData atkData)
    {
        var dmg = atkData.damage.Value;
        var r = HP - dmg;
        return !IsIgnoreHit && r <= 0;
    }
}

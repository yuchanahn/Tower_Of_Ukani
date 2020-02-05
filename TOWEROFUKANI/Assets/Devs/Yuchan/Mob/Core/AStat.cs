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
    MobDamageText Dmg = null;
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

        PlayerStatus.Inst.AddEffect(GetComponent<Mob_Base>(), MobAction.Hit, new StatusEffect(
            id,  MobAction.Hit, gameObject, StatusType.Debuff, 
            () => { Debug.Log("어떤 상태이상 시작."); } ,
            ()=> { Debug.Log("업뎃"); },  
            () => { Debug.Log("상태이상 끝"); }, endTime: 1)
        );
        PlayerStatus.Inst.Trigger(GetComponent<Mob_Base>(), MobAction.Hit);


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
        Dmg.ThisStart();


        
        MobHPBar.Show(gameObject, transform.position, 0.5f);
        return mHP;
    }

}

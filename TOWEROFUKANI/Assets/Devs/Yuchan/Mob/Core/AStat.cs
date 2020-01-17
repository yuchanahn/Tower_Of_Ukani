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
    public float HP { get => mHP; set { mHP = value; if (mHP <= 0) GetComponent<IHurt>().OnDead(); } }

    public float MAXHP { get => mMAXHP; }
    public float Damage => mDamage;

    private void Awake()
    {
        mMAXHP = mHP;
    }

    public float Hit(AttackData dmg)
    {
        if (IsIgnoreHit) return 0;
        HP -= dmg.damage.Value;
        mDmg += dmg.damage.Value;
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

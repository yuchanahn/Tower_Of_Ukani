using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AStat : MonoBehaviour, IDamage
{
    [SerializeField] private int mHP;
    private int mMAXHP;
    private int mDmg = 0;
    MobDamageText Dmg = null;
    public int HP { get => mHP; set { mHP = value; if (mHP <= 0) GetComponent<IHurt>().OnDead(); } }

    public int MAXHP { get => mMAXHP; }

    private void Awake()
    {
        mMAXHP = mHP;
    }

    public void Hit(int dmg)
    {
        HP -= dmg;
        mDmg += dmg;
        GetComponent<IHurt>().OnHurt();

        ATimer.SetAndReset(gameObject.name + GetInstanceID().ToString(), 0.1f,
        () =>
        {
            Dmg = null; mDmg = 0;
        });
        if (Dmg == null) Dmg = MobDamageText.Show(mDmg, transform.position);
        Dmg.mText.text = mDmg.ToString();
        Dmg.SetPoint(transform.position);
        Dmg.ThisStart();


        
        MobHPBar.Show(gameObject, transform.position, 0.5f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStat : MonoBehaviour, IDamage
{
    [SerializeField] private int mHP;
    private int mMAXHP;

    public int HP { get => mHP; set { mHP = value; if (mHP <= 0) GetComponent<IHurt>().OnDead(); } }

    public int MAXHP { get => mMAXHP; }

    private void Awake()
    {
        mMAXHP = mHP;
    }

    public void Hit(int dmg)
    {
        HP -= dmg;
        GetComponent<IHurt>().OnHurt();
        MobDamageText.Show(dmg, transform.position);
        MobHPBar.Show(gameObject, transform.position, 0.5f);
    }
}

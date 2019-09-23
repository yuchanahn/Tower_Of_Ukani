using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStat : MonoBehaviour, IDamage
{
    [SerializeField] private int mHP;

    public int HP { get => mHP; set { mHP = value; if (mHP <= 0) GetComponent<IHurt>().OnDead(); } }

    public void Hit(int dmg)
    {
        HP -= dmg;
        GetComponent<IHurt>().OnHurt();
    }
}

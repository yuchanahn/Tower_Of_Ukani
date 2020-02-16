using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_Task_ProjectileAttack : MonoBehaviour, ITask
{
    [SerializeField] AProjectile Projectile;
    [SerializeField] int Damage;
    [SerializeField] int Frequency;

    public bool Tick()
    {


        return true;
    }
}

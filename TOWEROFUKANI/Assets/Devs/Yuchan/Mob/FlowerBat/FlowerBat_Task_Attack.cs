using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBat_Task_Attack : MonoBehaviour, ITask
{
    Mob_FlowerBat mob;

    private void Awake()
    {
        mob = GetComponent<Mob_FlowerBat>();
    }

    public bool Tick()
    {
        mob.SetAni(eMobAniST.Attack);
        return true;
    }
}

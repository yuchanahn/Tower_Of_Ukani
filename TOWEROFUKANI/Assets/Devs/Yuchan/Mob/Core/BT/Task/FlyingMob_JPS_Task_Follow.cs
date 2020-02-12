using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMob_JPS_Task_Follow : MonoBehaviour, ITask
{
    FlyingMob_Base mob;

    private void Awake()
    {
        mob = GetComponent<FlyingMob_Base>();
    }

    public bool Tick()
    {
        return false;
    }
}

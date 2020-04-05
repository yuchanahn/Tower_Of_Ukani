using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSStatusEffect : MonoBehaviour, IVSForEach
{
    public void Create_Slow_nomal(GameObject o) => StatusEffect_Slow.CreateTest(o);

    public Action<GameObject> func()
    {
        return Create_Slow_nomal;
    }
}
